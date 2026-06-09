using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class Countdown
{
    extension(ICountdown card)
    {
        public void ResetCount()
        {
            card.CurrentCount = card.MaxCount;
        }

        public async Task DecrementCountAsync(int decrementBy = 1)
        {
            for (int i = decrementBy; (i > 0 && card.CurrentCount >  0); --i)
            {
                --card.CurrentCount;
                await DiceyHooks.OnAfterCardCountsDownAsync((RunState)card.Owner.RunState, (CardModel)card);
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
            await card.DecrementCountAsync(cardsDiscarded.Length);
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