using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Powers;

public class BefuddlePower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("CardName")];

    public CardModel? Card { get; set; }

    public void SetCards(CardModel card)
    {
        Card = card;
        CardCmd.ClearAffliction(card);
        ((StringVar)DynamicVars["CardName"]).StringValue = card.Title;
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner.Player)
        {
            return;
        }

        if (Card is not null)
        {
            List<CardModel> clones = [];
            for (int n = 0; n < Amount; ++n)
            {
                clones.Add(Card.CreateClone());

            }

            await CardPileCmd.AddGeneratedCardsToCombat(clones, PileType.Hand, player);
        }

        await PowerCmd.Remove(this);
    }
}