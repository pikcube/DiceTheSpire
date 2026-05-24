using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInventor.TheInventorCode.Gadgets;

public class MagicDice() : AbstractGadget(nameof(MagicDice))
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

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