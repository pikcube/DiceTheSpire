using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class BreathingExercisesQuest() : WorkoutQuestCard
{
    //Add a card to your deck that is already in your deck.
    public override CardModel RewardCard => BreathingExercises.Create();
}