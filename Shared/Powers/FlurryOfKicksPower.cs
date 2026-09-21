using MegaCrit.Sts2.Core.Entities.Powers;

namespace DiceTheSpire.Shared.Powers;

public class FlurryOfKicksPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

}
