using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace TheInventor.TheInventorCode.Gadgets;

public class LargeToolbox() : GadgetModel(nameof(LargeToolbox))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override decimal PowerBase => 3;

    public bool IsReady { get; set; }

    public override Task BeforeCombatStart()
    {
        IsReady = true;
        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player || !IsReady)
        {
            return Task.CompletedTask;
        }

        IsReady = false;
        return EmptyTheToolboxAsync(player);
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return EmptyTheToolboxAsync(player);
    }

    private async Task EmptyTheToolboxAsync(Player player)
    {
        Parent?.Flash();
        IEnumerable<CardModel> validColorlessCards = ModelDb.CardPool<ColorlessCardPool>()
            .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint);

        IEnumerable<CardModel> toolBoxCards = CardFactory.GetDistinctForCombat(player, validColorlessCards, Power, player.RunState.Rng.CombatCardGeneration);

        foreach (CardModel card in toolBoxCards)
        {
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
        }
    }
}