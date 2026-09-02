using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Powers;


public class ResonancePower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier,
        CardModel? cardSource)
    {
        if (applier != Owner || target != Owner || Owner.Player is null || power.GetTypeForAmount(amount) != PowerType.Debuff)
        {
            return;
        }

        if (power.GetTypeForAmount(amount) == power.GetTypeForAmount(-amount) && amount < 0)
        {
            return;
        }

        HookPlayerChoiceContext choiceContext = new(Owner.Player, LocalContext.NetId ?? 0, GameActionType.Combat);

        foreach (Creature creature in CombatState.Enemies)
        {
            for (int n = 0; n < Amount; ++n)
            {
                await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, CombatState.RunState, creature, Owner, null);
            }
        }
    }
}