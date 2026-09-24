using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;


public class ResonancePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier,
        CardModel? cardSource)
    {
        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
            return;
        }

        if (!InventorHelperFunctions.IsSelfDebuff(Owner, power, target, amount, applier) || power is ITemporaryPower)
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