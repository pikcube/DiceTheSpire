using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class ShockPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Debuff;

    public List<CardModel> Cards { get; set; } = [];

    protected override void AfterCloned()
    {
        base.AfterCloned();
        Cards = [];
    }

    public override PowerStackType StackType => PowerStackType.Counter;

    public override LocString Description
    {
        get
        {
            LocString val = base.Description;
            return val;
        }
    }

    private int StacksToResolve { get; set; }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (power != this)
        {
            return;
        }

        if (Owner.Player is null)
        {
            await VulnerablePower.ApplyAsync(choiceContext, Owner, amount, applier, cardSource, true);
            await PowerCmd.Remove(this);
            return;
        }

        if (CombatState.CurrentSide != CombatSide.Player)
        {
            StacksToResolve += (int)amount;
            return;
        }

        await ShockAsync(choiceContext, amount);
    }

    private async Task ShockAsync(PlayerChoiceContext choiceContext, decimal amount)
    {
        if (Owner.Player is null)
        {
            await VulnerablePower.ApplyAsync(choiceContext, Owner, amount, Applier, null, true);
            await PowerCmd.Remove(this);
            return;
        }

        CardModel[] cards = GetCards((int)amount, Owner.Player);

        foreach (CardModel card in cards)
        {
            card.AddTempKeyword(CardKeyword.Unplayable, this);
            Cards.Add(card);
        }

        foreach (CardModel card in cards)
        {
            await DiceyHooks.OnCardShocked(choiceContext, this, card);
        }
    }

    private static CardModel[] GetCards(int amount, Player player)
    {
        CardModel[] handPile =
        [
            .. PileType.Hand.GetPile(player).Cards
                .Where(c => !c.Keywords.Contains(CardKeyword.Unplayable))
        ];

        player.RunState.Rng.CombatCardSelection.Shuffle(handPile);

        return [..handPile.OrderBy(c => c.Keywords.Contains(CardKeyword.Ethereal)).Take(amount)];
    }

    //public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    //{
    //    return Cards.Contains(card);
    //}

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (Cards.Contains(card))
        {
            modifiedCost = -1;
            return true;
        }

        modifiedCost = originalCost;
        return false;
    }

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner)
        {
            return;
        }
        await ShockAsync(choiceContext, StacksToResolve);
        StacksToResolve = 0;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            Cards.Clear();
            await PowerCmd.Remove(this);
            TempKeywordManager.DestroyKeywordsEarly(this);
        }
    }
}