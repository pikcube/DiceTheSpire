using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace TheThief.TheThiefCode.Powers;

public class SecretWeaponPower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private decimal _amountToApply;

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier,
        out decimal modifiedAmount)
    {
        if (target != Owner || canonicalPower.Type != PowerType.Buff || canonicalPower is not ITemporaryPower)
        {
            modifiedAmount = amount;
            return false;
        }

        modifiedAmount = Math.Max(amount - this.Amount, 0);
        _amountToApply = Math.Min(Amount, amount);
        return true;
    }

    public override async Task AfterModifyingPowerAmountReceived(PowerModel power)
    {
        if (power is not ITemporaryPower tempPower)
        {
            return;
        }
        await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), tempPower.InternallyAppliedPower, Owner, _amountToApply, power.Applier, null, true);
    }

}