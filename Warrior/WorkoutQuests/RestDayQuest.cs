using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class RestDayQuest : WorkoutQuestCard, IAfterRestingListener
{
    //Rest at a restsite
    public override CardModel RewardCard => RestDay.Create();

    public async Task BetterAfterRestingAsync(Player player)
    {
        if (player == Owner)
        {
            await OnWorkoutCompleteAsync();
        }
    }
}