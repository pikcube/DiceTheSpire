using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class DrainingPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
        {
            return;
        }

        IReadOnlyList<Creature> targets = Owner.Side == CombatSide.Player ? CombatState.Enemies : CombatState.PlayerCreatures;

        IEnumerable<DamageResult> results = await CreatureCmd.Damage(choiceContext, targets, Amount, DamageProps.nonCardUnpowered, Owner);

        await CreatureCmd.Heal(Owner, results.Sum(r => r.UnblockedDamage));

        await PowerCmd.Decrement(this);
    }
}