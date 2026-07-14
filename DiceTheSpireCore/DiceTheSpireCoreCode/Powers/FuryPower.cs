using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
public class FuryPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        //return card.Owner.Creature == this.Owner ? playCount: playCount + 1;
        //|| CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>)(e => e.Actor == this.Owner && e.HappenedThisTurn(this.CombatState))) >= this.Amount
        return card.Owner.Creature != this.Owner  ? playCount : playCount + 1;
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        this.Flash();
        await PowerCmd.Decrement(this);
    }
}
