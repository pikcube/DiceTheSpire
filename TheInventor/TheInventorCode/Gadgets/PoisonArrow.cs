using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Gadgets;

public class PoisonArrow() : GadgetModel(nameof(PoisonArrow))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player || Parent?.Owner.Creature.CombatState is null)
        {
            return Task.CompletedTask;
        }

        return PoisonPower.ApplyAsync(choiceContext, Parent.Owner.Creature.CombatState.Enemies, 3, Parent?.Owner.Creature, null);
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }
}