using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

public class MagicSpanner() : GadgetModel(nameof(MagicSpanner))
{
    private bool IsUsedUp { get; set; }
    public override decimal PowerBase => 2;

    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override Task BeforeCombatStart()
    {
        IsUsedUp = false;
        return Task.CompletedTask;
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (IsUsedUp || Parent?.Owner != player)
        {
            return count;
        }

        Parent?.Flash();
        IsUsedUp = true;

        return count + Power;
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Parent?.Owner)
        {
            Parent?.Flash();
            return CardPileCmd.Draw(choiceContext, Power, player);
        }

        return Task.CompletedTask;
    }
}