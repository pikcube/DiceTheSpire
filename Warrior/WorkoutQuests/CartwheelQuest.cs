using MegaCrit.Sts2.Core.Models;
using DiceTheSpire.Warrior.WorkoutRewards;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;

public class CartwheelQuest() : WorkoutQuestCard
{
    //Add 5 cards to your deck
    public override CardModel RewardCard => Cartwheel.Create();
}