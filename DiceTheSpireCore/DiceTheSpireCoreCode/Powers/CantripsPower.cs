using DiceTheSpireCore.DiceTheSpireCoreCode.Commands;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;


public class CantripsPower : DiceTheSpireCorePower, IAfterRerollListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task AfterRerollAsync(CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
    {
        CantripsPower cantripsPower = this;
        IReadOnlyList<Creature> hittableEnemies = cantripsPower.CombatState.HittableEnemies;
        Creature? item = Owner.Player?.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
        if (hittableEnemies.Count == 0 || hittableEnemies is null || Owner.Player is null || Owner.Player.RunState is null || item is null)
            return;
        if (hittableEnemies.Count != 0)
        {
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), item, base.Amount, ValueProp.Unpowered | ValueProp.Unpowered, null, null, null);
            return;
        }
        return;

    }

}

