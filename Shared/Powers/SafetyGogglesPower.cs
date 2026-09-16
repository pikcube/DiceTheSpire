using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;

public class SafetyGogglesPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier,
        out decimal modifiedAmount)
    {
        if (target != Owner || applier != Owner || canonicalPower.GetTypeForAmount(amount) != PowerType.Debuff || amount == 0 || Owner.HasPower<ArtifactPower>() || InventorHelperFunctions.IsDebuffBeingRemoved(canonicalPower, amount))
        {
            modifiedAmount = amount;
            return false;
        }


        //todo: Powers that are not debuffs when negative but still have temporary variants
        modifiedAmount = 0;
        return true;

    }

    public override Task AfterModifyingPowerAmountReceived(PowerModel power)
    {
        Flash();
        return PowerCmd.Decrement(this);
    }
}