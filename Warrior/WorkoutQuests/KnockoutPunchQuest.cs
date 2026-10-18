using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class KnockoutPunchQuest : WorkoutQuestCard
{
    //Defeat an Elite
    public override CardModel RewardCard => KnockoutPunch.Create();
}