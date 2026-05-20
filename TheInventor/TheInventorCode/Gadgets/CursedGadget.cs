using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Powers;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class CursedGadget() : AbstractGadget(nameof(CursedGadget))
{
    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner == player)
        {
            await PowerCmd.Apply<CursedPower>(choiceContext, player.Creature, 1, null, null);
        }
    }
}