using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Keywords;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;

public class InspectModel() : CustomSingletonModel(HookType.Combat)
{
    public static async Task<int> InspectAsync(PlayerChoiceContext choiceContext, Player player, int cards)
    {
        CardModel[] topCards = [.. PileType.Draw.GetPile(player).Cards.Take(cards)];

        if (topCards.Length == 0)
        {
            return 0;
        }

        CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_BLINK"), 0, topCards.Length);

        CardModel[] selectedCards = [.. await CardSelectCmd.FromSimpleGrid(choiceContext, topCards, player, prefs)];

        await CardPileCmd.Add(selectedCards, PileType.Exhaust, skipVisuals: true);
        foreach (CardModel card in selectedCards)
        {
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top, skipVisuals: true);
            await BlinkModel.BlinkCardAsync(choiceContext, card);
        }


        foreach (IOnInspectListener listener in player.RunState.IterateHookListeners(player.Creature.CombatState).OfType<IOnInspectListener>())
        {
            await listener.OnInspectAsync(choiceContext, cards, selectedCards, player);
        }

        return selectedCards.Length;
    }
}