using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Powers;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class CursedGadget() : GadgetModel(nameof(CursedGadget))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override bool IsAllowedAsTempGadget => false;

    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner == player)
        {
            await PowerCmd.Apply<CursedPower>(choiceContext, player.Creature, 1 * GetPower(player), null, null);
        }
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStartEarly(choiceContext, player);
    }
}