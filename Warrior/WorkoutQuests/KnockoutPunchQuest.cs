using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class KnockoutPunchQuest : WorkoutQuestCard
{
    //Defeat an Elite
    public override CardModel RewardCard => KnockoutPunch.Create();

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        if (room.RoomType == RoomType.Elite)
        {
            await OnWorkoutCompleteAsync();
        }
    }
}