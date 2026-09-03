using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Inventor.Gadgets;

public class MagicDice() : GadgetModel(nameof(MagicDice))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != Parent?.Owner)
        {
            return amount;
        }

        return amount + DynamicVars.Energy.IntValue * Power;
    }

    public override Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Parent?.Owner)
        {
            Parent?.Flash();
        }

        return Task.CompletedTask;
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player)
        {
            return Task.CompletedTask;
        }

        Parent?.Flash();
        return PlayerCmd.GainEnergy(Power, player);

    }
}