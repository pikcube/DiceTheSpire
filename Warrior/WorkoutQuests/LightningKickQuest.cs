using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class LightningKickQuest : WorkoutQuestCard
{
    //Clear 5 combats
    public override CardModel RewardCard => LightningKick.Create();
}