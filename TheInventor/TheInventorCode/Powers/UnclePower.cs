using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Powers;
public class UnclePower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new IntVar("Delay", 5), new DamageVar(90, DamageProps.nonCardHpLoss)];

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Amount == 1)
        {
            await CreatureCmd.Damage(choiceContext, CombatState.Enemies, DynamicVars.Damage, Owner, null);
            await PowerCmd.Remove(this);
            return;
        }

        await PowerCmd.Decrement(this);
    }
}