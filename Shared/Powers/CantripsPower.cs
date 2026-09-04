using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Powers;


public class CantripsPower : DiceTheSpireCorePower, IAfterRerollListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task AfterRerollAsync(CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
    {
        CantripsPower cantripsPower = this;
        IReadOnlyList<Creature> hittableEnemies = cantripsPower.CombatState.HittableEnemies;
        Creature? item = Owner.Player?.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
        if (hittableEnemies.Count == 0 || Owner.Player is null || item is null)
        {
            return;
        }

        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), item, Amount, ValueProp.Unpowered | ValueProp.Unpowered, null, null, null);
    }

}

