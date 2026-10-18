using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class SomersaultQuest() : WorkoutQuestCard
{
    //Enter 4 ? rooms
    public override CardModel RewardCard => Somersault.Create();
}