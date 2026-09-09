using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Inventor.Token;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.Inventor.Uncommon;

public class EmptySlot() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(PersistenceOfMemory);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Unplayable), HoverTipFactory.Static(BetterStaticHoverTips.Held)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (RunState is null || CombatState is null)
        {
            return;
        }

        CardModel card = ModelDb.CardPool<TheInventorCardPool>()
            .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            .Where(c => c.Keywords.Contains(CardKeyword.Unplayable) && c.HasTurnEndInHandEffect)
            .TakeRandom(1, RunState.Rng.CombatCardGeneration)
            .FirstOrDefault() ?? Rock.Create();

        card = card.StrongMutableClone();
        CombatState.AddCard(card, Owner);
        card.SetToFreeThisTurn();
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
        AddKeyword(BlinkModel.Blink);
    }

    public override bool ModifyScrap()
    {
        return true;
    }
}