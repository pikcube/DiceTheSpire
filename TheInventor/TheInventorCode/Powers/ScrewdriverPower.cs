using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TheInventor.TheInventorCode.Powers;


public class ScrewdriverPower : TheInventorPower, IGadgetPowerListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public decimal ModifyGadgetPowerMultiplicative(Player owner)
    {
        return Owner == owner.Creature ? 2 : 1;
    }
}

public interface IGadgetPowerListener
{
    public decimal ModifyGadgetPowerMultiplicative(Player owner);
}