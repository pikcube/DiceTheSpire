using System.Data;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Commands;

[HarmonyPatch]
public static class InspectCmd
{
    public static async Task<int> InspectAsync(PlayerChoiceContext choiceContext, Player player, int cards)
    {
        CardPile drawPile = PileType.Draw.GetPile(player);
        CardModel[] selectedCards = [.. await InspectFromCombatPile(choiceContext, drawPile, player, new CardSelectorPrefs(DiceySelection.ToShock, 0, cards), c => drawPile.Cards.IndexOf(c) < cards)];

        await CardPileCmd.Add(selectedCards, PileType.Exhaust, skipVisuals: true);
        foreach (CardModel card in selectedCards)
        {
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top, skipVisuals: true);
            await ShockModel.ShockCardAsync(choiceContext, card);
        }

        foreach (IAfterInspectListener listener in player.RunState.IterateHookListeners(player.Creature.CombatState).OfType<IAfterInspectListener>())
        {
            await listener.AfterInspectAsync(choiceContext, cards, selectedCards, player);
        }

        return selectedCards.Length;
    }
    //Thanks lamali
    //todo: swap with baselib version when that is released.
    private static async Task<IEnumerable<CardModel>> InspectFromCombatPile(PlayerChoiceContext context, CardPile pile,
        Player player, CardSelectorPrefs prefs, Func<CardModel, bool>? filter)
    {
        if (CombatManager.Instance.IsEnding)
        {
            return [];
        }

        if (!pile.IsCombatPile)
        {
            throw new InvalidOperationException("Cannot perform on a non combat pile");
        }

        if (CombatManager.Instance.IsPlayerReadyToEndTurn(player)
            && player.Creature.CombatState is { CurrentSide: CombatSide.Player })
        {
            CombatManager.Instance.UndoReadyToEndTurn(player);
        }

        uint? choiceId = null;
        if (CardSelectCmd.Selector == null)
        {
            choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
            await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
        }

        List<CardModel> cards = [.. filter == null ? pile.Cards : pile.Cards.Where(filter)];
        List<CardModel> result;
        if (cards.Count == 0)
        {
            result = [];
        }
        else if (!prefs.RequireManualConfirmation && cards.Count <= prefs.MinSelect)
        {
            result = cards;
        }
        else if (CardSelectCmd.Selector != null)
        {
            result = [.. await CardSelectCmd.Selector.GetSelectedCards(cards, prefs.MinSelect, prefs.MaxSelect)];
        }
        else if (TestMode.IsOn)
        {
            result = [.. cards.Take(prefs.MinSelect)];
        }
        else if (LocalContext.IsMe(player) && RunManager.Instance.NetService.Type != NetGameType.Replay)
        {
            NPlayerHand.Instance?.CancelAllCardPlay();
            NCombatPileCardSelectScreen screen = NCombatPileCardSelectScreen.Create(pile, prefs, filter);
            NOverlayStack.Instance?.Push(screen);
            Action? stopKeepingOrder = null;
            try
            {
                if (pile.Type == PileType.Draw)
                {
                    stopKeepingOrder = KeepPileOrder(screen, pile, filter);
                }

                result = [.. await screen.CardsSelected()];
            }
            finally
            {
                stopKeepingOrder?.Invoke();
            }
            RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId!.Value,
                PlayerChoiceResult.FromMutableCombatCards(result));
        }
        else
        {
            result = [..(await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId!.Value)).AsCombatCards()];
        }

        if (choiceId.HasValue)
        {
            await context.SignalPlayerChoiceEnded();
        }

        return result;
    }

    private static Action KeepPileOrder(NCombatPileCardSelectScreen screen, CardPile pile, Func<CardModel, bool>? filter)
    {
        NCardGrid grid = screen.PrivateFieldWrapper<NCardGridSelectionScreen, NCardGrid>("_grid").Value ?? throw new NoNullAllowedException();
        Reorder();
        pile.ContentsChanged += Reorder;
        return () => pile.ContentsChanged -= Reorder;

        void Reorder()
        {
            if (!GodotObject.IsInstanceValid(grid))
            {
                return;
            }

            List<CardModel> cards = [.. filter == null ? pile.Cards : pile.Cards.Where(filter)];
            if (cards.Count == 0)
            {
                return;
            }

            grid.SetCards(cards, pile.Type, [SortingOrders.Ascending]);
            foreach (CardModel card in screen.PrivateFieldWrapper<NCombatPileCardSelectScreen, HashSet<CardModel>>("_selectedCards").Value?.Where(cards.Contains) ?? [])
            {
                grid.HighlightCard(card);
            }
        }
    }
}