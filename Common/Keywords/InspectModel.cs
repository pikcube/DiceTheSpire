using BaseLib.Abstracts;
using DiceTheSpire.Common.Listeners;
using DiceTheSpire.Common.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Keywords;
using ICardSelector = MegaCrit.Sts2.Core.TestSupport.ICardSelector;

namespace DiceTheSpire.Common.Keywords;

[UsedImplicitly]
public class InspectModel() : CustomSingletonModel(HookType.Combat)
{
    public static async Task<int> InspectAsync(PlayerChoiceContext choiceContext, Player player, int cards)
    { 
        CardModel[] selectedCards = [.. await FromGridForInspectAsync(choiceContext, cards, player)];

        await CardPileCmd.Add(selectedCards, PileType.Exhaust, skipVisuals: true);
        foreach (CardModel card in selectedCards)
        {
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top, skipVisuals: true);
            await BlinkModel.BlinkCardAsync(choiceContext, card);
        }

        foreach (IAfterInspectListener listener in player.RunState.IterateHookListeners(player.Creature.CombatState).OfType<IAfterInspectListener>())
        {
            await listener.AfterInspectAsync(choiceContext, cards, selectedCards, player);
        }

        return selectedCards.Length;
    }

    private static async Task<IEnumerable<CardModel>> FromGridForInspectAsync(PlayerChoiceContext context, int count, Player player)
    {
        //Don't select anything if combat is ending or if the total to inspect is 0
        if (CombatManager.Instance.IsEnding || count < 1)
        {
            return [];
        }

        //In case of Vakuu, let him take the wheel
        if (CardSelectCmd.Selector is not null)
        {
            return await DoSelectorInspectAsync(CardSelectCmd.Selector, player, count);
        }

        //Start a choice and run the inspection
        uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
        await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);

        //Warning! Do not enumerate this until after the player choice has begun or you will get multiplayer weirdness
        List<CardModel> cards = [.. PileType.Draw.GetPile(player).Cards.Take(count)];
        List<CardModel> result = [.. await DispatchForInspectAsync(choiceId, count, cards, player)];
        
        await context.SignalPlayerChoiceEnded();
        
        return result;
    }

    private static async Task<IEnumerable<CardModel>> DispatchForInspectAsync(uint choiceId, int count, List<CardModel> cards, Player player)
    {
        if (cards.Count < 1)
        {
            return [];
        }

        if (cards.Count < count)
        {
            count = cards.Count;
        }

        //If we aren't the local player, we just listen for the choice to come in over the network
        if (!LocalContext.IsMe(player) || RunManager.Instance.NetService.Type == NetGameType.Replay)
        {
            return await DoRemoteInspectAsync(choiceId, player, cards);
        }

        //This will never trigger unless Megacrit runs their test suite with my mod loaded, but it was easy enough to add in
        if (CardSelectCmd.LocalSelector is not null)
        {
            return await DoSelectorInspectAsync(CardSelectCmd.LocalSelector, player, count);
        }

        //Otherwise Pull up the card selection UI
        return await DoLocalInspectAsync(choiceId, count, player, cards);
    }

    private static async Task<IEnumerable<CardModel>> DoLocalInspectAsync(uint choiceId, int count, Player player, List<CardModel> cards)
    {
        NPlayerHand.Instance?.CancelAllCardPlay();
        NSimpleCardSelectScreen screen = NSimpleCardSelectScreen.Create(cards, new CardSelectorPrefs(DiceySelection.ToBlink, 0, count));
        NOverlayStack.Instance?.Push(screen);
        List<CardModel> result = [.. await screen.CardsSelected()];
        RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndexes([.. result.Select(c => cards.IndexOf(c))]));
        Log.Info($"Player {player.NetId} chose cards [{string.Join(",", result.Select<CardModel, string>(c => c.Id.Entry))}]");
        return result;
    }

    private static async Task<IEnumerable<CardModel>> DoRemoteInspectAsync(uint choiceId, Player player, List<CardModel> cards)
    {
        PlayerChoiceResult remoteChoice = await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId);
        List<CardModel> results = [..remoteChoice.AsIndexes().Select(i => cards[i])];
        Log.Info($"Player {player.NetId} chose cards [{string.Join(",", results.Select<CardModel, string>(c => c.Id.Entry))}]");
        return results;
    }

    private static async Task<IEnumerable<CardModel>> DoSelectorInspectAsync(ICardSelector selector, Player player, int count)
    {
        IEnumerable<CardModel> result = await selector.GetSelectedCards(PileType.Draw.GetPile(player).Cards.Take(count), 0, count);
        List<CardModel> inspectWithSelectorAsync = result as List<CardModel> ?? [.. result];
        Log.Info($"Player {player.NetId} chose cards [{string.Join(",", inspectWithSelectorAsync.Select<CardModel, string>(c => c.Id.Entry))}]");
        return inspectWithSelectorAsync;
    }
}