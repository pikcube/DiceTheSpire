using MegaCrit.Sts2.Core.Entities.Powers;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;
public class FlurryOfKicksPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
}
