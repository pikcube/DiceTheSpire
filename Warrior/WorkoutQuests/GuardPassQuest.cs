using MegaCrit.Sts2.Core.Models;
using DiceTheSpire.Warrior.WorkoutRewards;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class GuardPassQuest() : WorkoutQuestCard
{
    //Win a combat without blocking any damage
    public override CardModel RewardCard => GuardPass.Create();
}