using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Utility;

namespace TheThief.TheThiefCode.Powers;

public class StolenWeaponPower : TheThiefPower, IAfterPowerRemovedListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task AfterPowerRemovedAsync(PowerModel powerModel, Creature? oldOwner)
    {
        if (oldOwner != Owner || powerModel.Type != PowerType.Buff ||
            powerModel is not ITemporaryPower tempPower)
        {
            return;
        }
        await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), tempPower.InternallyAppliedPower, Owner, Math.Min(Amount, powerModel.Amount), Owner, null);

    }
}