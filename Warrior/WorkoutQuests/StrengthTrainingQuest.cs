using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class StrengthTrainingQuest() : WorkoutQuestCard
{
    //Add 3 powers to your deck
    public override CardModel RewardCard => StrengthTraining.Create();
}