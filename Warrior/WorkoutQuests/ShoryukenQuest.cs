using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class ShoryukenQuest : WorkoutQuestCard
{
    //Smith at a restsite
    public override CardModel RewardCard => Shoryuken.Create();

    public override async Task AfterRestSiteSmith(Player player)
    {
        if (player == Owner)
        {
            await OnWorkoutCompleteAsync();
        }
    }
}