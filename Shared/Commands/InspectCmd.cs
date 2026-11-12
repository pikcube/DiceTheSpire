using BaseLib.Commands;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Commands;

[HarmonyPatch]
public static class InspectCmd
{
    public static async Task<int> InspectAsync(PlayerChoiceContext choiceContext, Player player, int cards)
    {
        CardPile drawPile = PileType.Draw.GetPile(player);
        CardModel[] selectedCards = [.. await BaseLibCardSelectCmd.FromCombatPileInOrder(choiceContext, drawPile, player, new CardSelectorPrefs(DiceySelection.ToShock, 0, cards), c => drawPile.Cards.IndexOf(c) < cards)];

        await CardPileCmd.Add(selectedCards, PileType.Exhaust, skipVisuals: true);
        foreach (CardModel card in selectedCards)
        {
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top, skipVisuals: true);
            await ShockModel.ShockCardAsync(choiceContext, card);
        }

        await DiceyHooks.OnInspectAsync(player.RunState, player.Creature.CombatState, choiceContext, player, cards, selectedCards);

        return selectedCards.Length;
    }
}