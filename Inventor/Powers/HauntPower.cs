using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Powers;

public class HauntPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player is null)
        {
            //Nope, absolutely not, under no circumstances
            await PowerCmd.Remove(this);
        }
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Player player = cardPlay.Player;
        if (player.Creature != Owner)
        {
            return;
        }

        CardPile hand = PileType.Hand.GetPile(player);

        CardModel[] cardsToDiscard = [.. hand.Cards.TakeRandom(Amount, player.RunState.Rng.CombatCardSelection)];
        await CardCmd.DiscardAndDraw(choiceContext, cardsToDiscard, cardsToDiscard.Length);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
        {
            return;
        }
        await PowerCmd.Remove(this);
    }
}