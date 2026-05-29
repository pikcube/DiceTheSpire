using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

public class Protection() : GadgetModel(nameof(Protection))
{
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player)
        {
            return;
        }

        await PowerCmd.Apply<ReducePower>(choiceContext, player.Creature, 2, player.Creature, null);
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }
}