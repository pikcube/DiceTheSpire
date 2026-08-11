using MegaCrit.Sts2.Core.Entities.Powers;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
public class WarriorFireBreathPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

}
