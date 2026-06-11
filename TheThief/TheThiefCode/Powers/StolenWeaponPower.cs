using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheThief.TheThiefCode.Powers;

public class StolenWeaponPower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier,
        out decimal modifiedAmount)
    {
        if (target != Owner)
        {
            modifiedAmount = amount;
            return false;
        }

        var amountToReduceBy = Math.Min(Amount, Math.Abs(amount));
        switch (canonicalPower)
        {
            case StrengthPower when canonicalPower.GetTypeForAmount(amount) == PowerType.Debuff:
                modifiedAmount = amount + amountToReduceBy;
                return true;
            case TemporaryStrengthPower { Type: PowerType.Debuff }:
                modifiedAmount = amount - amountToReduceBy;
                return true;
            default:
                modifiedAmount = amount;
                return false;
        }
    }
}