using BaseLib.Abstracts;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Inventor.Gadgets;

public class Stardust() : GadgetModel(nameof(Stardust))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player || player.Creature.CombatState is null)
        {
            return;
        }

        Parent.Flash();

        for (int n = 0; n < Power; ++n)
        {
            Creature? target = player.Creature.CombatState.Enemies.TakeRandom(1, player.RunState.Rng.CombatTargets).SingleOrDefault();

            if (target is null)
            {
                return;
            }

            await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, player.RunState, target, player.Creature, null);
        }
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }
}