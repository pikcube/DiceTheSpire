using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

public class MagicDice() : AbstractGadget(nameof(MagicDice))
{
    public override decimal ModifyEnergyGain(Player player, decimal amount)
    {
        if (player == Parent?.Owner)
        {
            return amount + 1;
        }

        return amount;
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return Parent?.Owner == player ? PlayerCmd.GainEnergy(1, player) : Task.CompletedTask;
    }
}