using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

public class BigBomb() : GadgetModel(nameof(BigBomb))
{
    public override decimal PowerBase => 60;
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override bool IsAllowedAsTempGadget => false;

    public override void OnFirstCharge()
    {
        Parent?.SetValue(3);
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Parent?.Owner.Creature.CombatState is null || Parent?.Owner.PlayerCombatState is null || Parent.Owner.Creature.Side != side)
        {
            return;
        }

        if (!participants.Contains(Parent.Owner.Creature))
        {
            return;
        }

        int turnNumber = Parent.Owner.PlayerCombatState.TurnNumber;
        int display = 3 - turnNumber;
        if (display < 0)
        {
            display = 0;
        }

        Parent.SetValue(display);

        if (turnNumber != 3)
        {
            return;
        }

        Parent.Flash();
        await CreatureCmd.Damage(choiceContext, Parent.Owner.Creature.CombatState.Enemies, Power, DamageProps.nonCardHpLoss, Parent.Owner.Creature, null, null);
    }

    public override async Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Parent?.Owner || Parent?.Owner.PlayerCombatState is null || Parent.Owner.Creature.CombatState is null)
        {
            return;
        }

        if (Parent.Owner.PlayerCombatState.TurnNumber < 3)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, Parent.Owner.Creature.CombatState.Enemies, Power, DamageProps.nonCardHpLoss, Parent.Owner.Creature, null, null);
    }
}