using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Common.Powers;


public class ReducePower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        return target != Owner || props.HasFlag(ValueProp.Unblockable) ? 0 : -Amount;
    }

    public override Task AfterModifyingDamageAmount(CardModel? cardSource)
    {
        Flash();
        return Task.CompletedTask;
    }
}