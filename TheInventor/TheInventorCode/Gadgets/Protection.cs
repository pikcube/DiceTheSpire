using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

public class Protection() : GadgetModel(nameof(Protection))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override decimal PowerBase => 2;

    public bool IsUsedUp { get; set; }

    public override Task BeforeCombatStart()
    {
        IsUsedUp = false;
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (IsUsedUp || Parent?.Owner != player)
        {
            return;
        }

        Parent.Flash();
        await PowerCmd.Apply<ReducePower>(choiceContext, player.Creature, Power, player.Creature, null);
        IsUsedUp = true;
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }

    public override bool IsAllowedAsTempGadget => false;
}