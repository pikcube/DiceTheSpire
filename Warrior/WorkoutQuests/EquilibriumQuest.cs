using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class EquilibriumQuest() : WorkoutQuestCard
{
    //Win a combat without taking damage
    public override CardModel RewardCard => Equilibrium.Create();
}