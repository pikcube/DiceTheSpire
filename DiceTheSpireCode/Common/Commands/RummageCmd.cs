using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Commands;

public static class RummageCmd
{
    public static async Task RummageAsync(PlayerChoiceContext choiceContext, Player player, int amount, AbstractModel source)
    {
        CardSelectorPrefs cardSelectorPrefs = new(CardSelectorPrefs.DiscardSelectionPrompt, 0, amount);
        CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, player, cardSelectorPrefs, null, source)];
        foreach (CardModel card in cards)
        {
            await CardCmd.Discard(choiceContext, card);
        }

        if (cards.Length == 0)
        {
            return;
        }
        await CardPileCmd.Draw(choiceContext, cards.Length, player);
    }
}