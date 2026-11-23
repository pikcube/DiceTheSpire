using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Inventor.Gadgets;

public class Armageddon() : GadgetModel(nameof(Armageddon))
{
    public override decimal PowerBase => 1;

    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (combatState.RoundNumber > 1)
        {
            return;
        }

        foreach (Creature creature in combatState.Creatures)
        {
            await CreatureCmd.SetCurrentHp(creature, Power);
        }

        BreakMe();
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return Task.CompletedTask;
    }

    public override bool IsAllowedAsTempGadget => false;
}