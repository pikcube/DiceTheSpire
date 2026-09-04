using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class SafetyGogglesPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier,
        out decimal modifiedAmount)
    {
        if (target != Owner || applier != Owner || canonicalPower.Type != PowerType.Debuff)
        {
            modifiedAmount = amount;
            return false;
        }

        modifiedAmount = 0;
        return true;

    }

    public override Task AfterModifyingPowerAmountReceived(PowerModel power)
    {
        Flash();
        return PowerCmd.Decrement(this);
    }
}