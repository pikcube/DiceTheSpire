using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class PowerUp() : GadgetModel(nameof(PowerUp))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Parent?.Owner)
        {
            return;
        }

        await VigorPower.ApplyAsync(choiceContext, player.Creature, 6, player.Creature, null);
    }


    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }
}