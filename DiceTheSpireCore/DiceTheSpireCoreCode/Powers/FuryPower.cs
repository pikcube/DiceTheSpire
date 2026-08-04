using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class FuryPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        //return card.Owner.Creature == this.Owner ? playCount: playCount + 1;
        //|| CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>)(e => e.Actor == this.Owner && e.HappenedThisTurn(this.CombatState))) >= this.Amount
        if (card.Owner.Creature != Owner)
        {
            return playCount;
        }

        if (card is IFuryModifier { ShouldIgnoreFury: true })
        {
            return playCount;
        }

        return playCount + 1 + Owner.GetPower<FuriousFormPower>()?.Amount ?? 0;
        
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        if (card is IFuryModifier { ShouldMaintainFury: true })
        {
            return;
        }

        await PowerCmd.Decrement(this);
    }
}