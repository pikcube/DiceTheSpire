using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class DeepFreezeQuest : WorkoutQuestCard
{
    //Skip 2 card rewards
    public override CardModel RewardCard => DeepFreeze.Create();
}