using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class GuardPassQuest : WorkoutQuestCard
{
    //Win a combat without blocking any damage
    public override CardModel RewardCard => GuardPass.Create();
    [SavedProperty]
    public bool DamageBlocked
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            DamageBlocked = false;
        }

        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result,
        ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (result.BlockedDamage > 0 && result.Receiver == Owner.Creature)
        {
            DamageBlocked = true;
        }

        return Task.CompletedTask;
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        if (DamageBlocked)
        {
            return;
        }

        await OnWorkoutCompleteAsync();
    }
}