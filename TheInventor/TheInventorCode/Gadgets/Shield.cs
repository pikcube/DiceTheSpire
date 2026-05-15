using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

public class Shield() : AbstractGadget(nameof(Shield))
{
    public override string GadgetText => "Shield: At the start of each turn, gain [blue]5[/blue] [gold]block[/gold].";

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player)
        {
            return;
        }

        await CreatureCmd.GainBlock(player.Creature, new BlockVar(5, BlockProps.nonCardUnpowered), null);
    }
}