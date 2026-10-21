using DiceTheSpire.Shared.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Powers;


public class ShieldChargePower : DiceTheSpirePower
{

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Enemy)
        {
            return;
        }

        IReadOnlyList<Creature> hittableEnemies = this.CombatState.HittableEnemies;
        if (hittableEnemies.Count == 0 || Owner.Player is null)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, hittableEnemies, Owner.Player.Creature.Block, ValueProp.Unpowered | ValueProp.Unpowered, null, null, null);

        //HookPlayerChoiceContext hookPlayerChoiceContext = new(this, card.Owner.NetId, CombatState, GameActionType.Combat);
        //await CreatureCmd.Damage(hookPlayerChoiceContext, CombatState.Enemies, Amount, DamageProps.nonCardUnpowered, Owner, null, null);

        await PowerCmd.Decrement(this);

    }


}