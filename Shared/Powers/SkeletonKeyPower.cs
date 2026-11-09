using MegaCrit.Sts2.Core.Entities.Powers;

namespace DiceTheSpire.Shared.Powers;

//Is just a visual indicator. Logic is handled by SkeletonKey.cs
public class SkeletonKeyPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}