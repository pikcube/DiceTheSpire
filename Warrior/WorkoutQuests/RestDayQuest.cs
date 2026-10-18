using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class RestDayQuest() : WorkoutQuestCard
{
    //Rest at a restsite
    public override CardModel RewardCard => RestDay.Create();
}