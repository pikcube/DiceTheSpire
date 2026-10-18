using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;

public class CoreTrainingQuest() : WorkoutQuestCard
{
    //Add 4 skills to your deck
    public override CardModel RewardCard => CoreTraining.Create();
}