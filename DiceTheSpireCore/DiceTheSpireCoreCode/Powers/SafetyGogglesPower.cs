using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class SafetyGogglesPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier,
        out decimal modifiedAmount)
    {
        if (applier != Owner)
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
        return Task.CompletedTask;
    }

    public override Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        return side == Owner.Side ? PowerCmd.Decrement(this) : Task.CompletedTask;
    }
}