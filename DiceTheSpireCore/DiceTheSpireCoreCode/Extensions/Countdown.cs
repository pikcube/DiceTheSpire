using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class Countdown
{
    extension(ICountdown card)
    {
        public void ResetCount()
        {
            card.CurrentCount = card.MaxCount;
        }

        public void DecrementCount(int decrementBy = 1)
        {
            card.CurrentCount -= decrementBy;
            if (card.CurrentCount < 0)
            {
                card.CurrentCount = 0;
            }
        }

        public void UpgradeCountdown(int upgradeBy)
        {
            card.MaxCount += upgradeBy;
        }
    }

    public static async Task OnCountdownPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay, ICountdown card)
    {
        if (card.CurrentCount > 0)
        {
            CardModel[] cardsDiscarded =
            [
                ..await CardSelectCmd.FromHandForDiscard(choiceContext, card.Owner,
                    new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt,
                        0, card.CurrentCount), null, (AbstractModel)card)
            ];
            await CardCmd.Discard(choiceContext, cardsDiscarded);
            card.DecrementCount(cardsDiscarded.Length);
        }

        if (card.CurrentCount == 0)
        {
            await card.OnCountdownZero(choiceContext, cardPlay);
            card.ResetCount();
        }
        else if (cardPlay.Card.Type == CardType.Power)
        {
            await CardPileCmd.Add(cardPlay.Card, PileType.Discard);
        }
    }
}