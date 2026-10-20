using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class BreathingExercisesQuest : WorkoutQuestCard
{
    //Add a card to your deck that is already in your deck.
    public override CardModel RewardCard => BreathingExercises.Create();
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Owner || card.Pile?.Type != PileType.Deck)
        {
            return;
        }

        if (Owner.Deck.Cards.Any(c => c != card && c.CanonicalInstance == card.CanonicalInstance))
        {
            await OnWorkoutCompleteAsync();
        }
    }
}