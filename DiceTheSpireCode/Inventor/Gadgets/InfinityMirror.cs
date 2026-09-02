using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

public class InfinityMirror() : GadgetModel(nameof(InfinityMirror))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != Parent?.Owner || player.Creature.CombatState is null)
        {
            return amount;
        }
        Parent.Flash();
        return amount + player.Creature.CombatState.RoundNumber * GetPower(player);
    }

    public override async Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState is null)
        {
            return;
        }

        Parent?.Flash();
        await PlayerCmd.GainEnergy(player.Creature.CombatState.RoundNumber * Power, player);
    }

    public override bool IsAllowedAsTempGadget => false;
}