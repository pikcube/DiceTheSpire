using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class EnduranceTrainingQuest : WorkoutQuestCard
{
    //Climb 8 floors
    public override CardModel RewardCard => EnduranceTraining.Create();
}