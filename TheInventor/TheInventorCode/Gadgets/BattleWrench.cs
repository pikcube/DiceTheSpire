using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class BattleWrench() : GadgetModel(nameof(BattleWrench))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override decimal PowerBase => 2;

    public override decimal ModifyHandDrawLate(Player player, decimal count)
    {
        if (player == Parent?.Owner)
        {
            return count + Power;
        }

        return count;
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Parent?.Owner)
        {
            return CardPileCmd.Draw(choiceContext, Power, player);
        }

        return Task.CompletedTask;
    }
}