using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

public class Crack() : AbstractGadget(nameof(Crack))
{
    public bool IsReady { get; set; } = false;

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, ICombatState combatState)
    {
        if (side == CombatSide.Player)
        {
            IsReady = true;
        }

        return Task.CompletedTask;
    }

    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        Player? owner = Parent?.Owner;
        ICombatState? combatState = Parent?.Owner.Creature.CombatState;
        if (!IsReady || delta >= 0 || combatState is null || owner is null)
        {
            return;
        }

        IsReady = false;

        HookPlayerChoiceContext choiceContext = new(owner, owner.NetId, GameActionType.Combat);

        await CreatureCmd.Damage(choiceContext, combatState.Enemies, new DamageVar(-delta, DamageProps.nonCardHpLoss),
            null, null);
    }
}