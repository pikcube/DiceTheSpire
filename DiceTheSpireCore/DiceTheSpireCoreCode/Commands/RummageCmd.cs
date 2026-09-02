using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Commands
{
    public static class RummageCmd
    {
        public static async Task RummageAsync(int amount, PlayerChoiceContext choiceContext, Creature owner, CardModel source)
        {
            if(owner.Player is null)
            {
                return;
            }

            CardSelectorPrefs cardSelectorPrefs = new(CardSelectorPrefs.DiscardSelectionPrompt, 0, amount);
            CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, owner.Player, cardSelectorPrefs, null, source)];
            foreach (CardModel card in cards)
            {
                await CardCmd.Discard(choiceContext, card);
            }

            if (cards.Length == 0)
            {
                return;
            }
            await CardPileCmd.Draw(choiceContext, cards.Length, owner.Player);
        }


    }
}
