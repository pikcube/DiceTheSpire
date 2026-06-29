using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards.Token;

namespace TheInventor.TheInventorCode.Gadgets;

public class Rockslide() : GadgetModel(nameof(Rockslide))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override decimal PowerBase => 2;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Parent?.Owner && player.Creature.CombatState is not null)
        {
            for (int n = 0; n < Power; ++n)
            {
                await CardPileCmd.AddGeneratedCardToCombat(Rock.Create(player, player.Creature.CombatState), PileType.Hand, player);
            }
        }
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }
}