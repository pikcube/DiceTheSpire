using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Gadgets;

public class AutoBump() : AbstractGadget(nameof(AutoBump))
{
    public override string GadgetText => "Auto Bump: At the start of each turn, upgrade a card for the rest of combat.";

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player)
        {
            return;
        }

        CardModel? card = await CardSelectCmd.FromHandForUpgrade(choiceContext, player, Parent);

        if (card == null)
        {
            return;
        }

        CardCmd.Upgrade(card);
    }
}