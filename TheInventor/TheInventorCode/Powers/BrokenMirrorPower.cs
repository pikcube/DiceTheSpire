using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInventor.TheInventorCode.Powers;

public class BrokenMirrorPower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player == Owner.Player ? amount + Amount : amount;
    }
}