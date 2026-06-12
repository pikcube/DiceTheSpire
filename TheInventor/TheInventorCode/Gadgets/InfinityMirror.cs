using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

public class InfinityMirror() : GadgetModel(nameof(InfinityMirror))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != Parent?.Owner || player.Creature.CombatState is null)
        {
            return amount;
        }

        return amount + player.Creature.CombatState.RoundNumber;
    }

    public override async Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState is null)
        {
            return;
        }
        await PlayerCmd.GainEnergy(player.Creature.CombatState.RoundNumber, player);
    }

    public override bool IsAllowedAsTempGadget => false;
}