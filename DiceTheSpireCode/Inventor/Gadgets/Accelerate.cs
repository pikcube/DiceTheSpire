using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Common.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

public class Accelerate() : GadgetModel(nameof(Accelerate))
{
    public override decimal PowerBase => 4;
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (Parent?.Owner != player)
        {
            return;
        }

        Parent.Flash();

        await Parent.Owner.InspectAsync(choiceContext, Power);
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return player.Creature.CombatState is null ? Task.CompletedTask : BeforeHandDraw(player, choiceContext, player.Creature.CombatState);
    }
}