using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInventor.TheInventorCode.Gadgets;

public class Burrower() : GadgetModel(nameof(Burrower))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override bool IsAllowedAsTempGadget => false;
    public bool IsUsedUp { get; set; }

    public override decimal PowerBase => 5;

    public override Task BeforeCombatStart()
    {
        IsUsedUp = false;
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (IsUsedUp || player != Parent?.Owner || player.Creature.CombatState is null)
        {
            return;
        }

        IsUsedUp = true;

        for (int n = 0; n < Power; ++n)
        {
            foreach (Creature c in player.Creature.CombatState.Enemies)
            {
                Parent.Flash();
                await PowerCmd.Apply<WeakPower>(choiceContext, c, 1, null, null);
            }
        }
    }

    public override async Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        IsUsedUp = false;
        await AfterPlayerTurnStart(choiceContext, player);
    }
}