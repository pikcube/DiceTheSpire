using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Utility;

namespace DiceTheSpire.Common.Powers;

public class StolenWeaponPower : TheThiefPower, IAfterPowerRemovedListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task AfterPowerRemovedAsync(PowerModel powerModel, Creature? oldOwner)
    {
        if (oldOwner != Owner || powerModel is not StrengthPower)
        {
            return;
        }

        Flash();
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, Math.Min(Amount, powerModel.Amount), Owner,
            null);
    }

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
                Flash();
                modifiedAmount = amount + amountToReduceBy;
                return true;
            case TemporaryStrengthPower { Type: PowerType.Debuff }:
                Flash();
                modifiedAmount = amount - amountToReduceBy;
                return true;
            default:
                modifiedAmount = amount;
                return false;
        }
    }
}