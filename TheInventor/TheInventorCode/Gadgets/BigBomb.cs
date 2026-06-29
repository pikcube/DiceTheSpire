using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

public class BigBomb() : GadgetModel(nameof(BigBomb))
{
    public override decimal PowerBase => 60;
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    private int TurnNumber { get; set; }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Parent?.Owner.Creature.Side || Parent.Owner.Creature.CombatState is null)
        {
            return;
        }

        ++TurnNumber;

        switch (TurnNumber)
        {
            case 3:
                await CreatureCmd.Damage(choiceContext, Parent.Owner.Creature.CombatState.Enemies, Power, DamageProps.nonCardHpLoss, Parent.Owner.Creature, null);
                break;
        }
    }
}