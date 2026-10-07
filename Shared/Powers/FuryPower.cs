using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class FuryPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != Owner || card is IFuryModifier { ShouldIgnoreFury: true })
        {
            return playCount;
        }

        return playCount + Amount;

    }

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Player.Creature != Owner || cardPlay.Card is IFuryModifier {ShouldMaintainFury: true} or IFuryModifier {ShouldIgnoreFury: true})
        {
            return;
        }

        if (cardPlay.IsLastInSeries)
        {
            await PowerCmd.Remove(this);
        }
        else if (Amount > 0)
        {
            await PowerCmd.Decrement(this);
        }
    }
}