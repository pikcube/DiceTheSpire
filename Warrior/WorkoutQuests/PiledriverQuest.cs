using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class PiledriverQuest() : WorkoutQuestCard
{
    //Add 4 attacks to your deck
    public override CardModel RewardCard => Piledriver.Create();
}