using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Powers;


public class ResonancePower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        IRunState runState = CombatState.RunState;
        if (applier != Owner || power.Owner != Owner || power.Type != PowerType.Debuff)
        {
            return;
        }

        foreach (Creature target in CombatState.Enemies)
        {
            for (int n = 0; n < Amount; ++n)
            {
                await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, runState, target, Owner, null);
            }
        }
    }
}