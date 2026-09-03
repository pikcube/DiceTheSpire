using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Common.Powers;


public class FrozenGashPower : DiceTheSpireCorePower
{

    public override PowerType Type => PowerType.Debuff;
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;
    public override PowerStackType StackType => PowerStackType.Single;


    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Enemy || Applier?.Player is null)
        {
            return;
        }
        await CreatureCmd.Damage(choiceContext, Owner, Applier.Player.Creature.Block, DamageProps.nonCardHpLoss, null, null);
    }

}