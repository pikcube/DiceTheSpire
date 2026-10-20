using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class EquilibriumQuest : WorkoutQuestCard
{
    //Win a combat without taking damage
    public override CardModel RewardCard => Equilibrium.Create();

    [SavedProperty]
    public bool DamageTaken
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
            DamageTaken = false;
        }

        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result,
        ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (result.UnblockedDamage > 0 && result.Receiver == Owner.Creature)
        {
            DamageTaken = true;
        }

        return Task.CompletedTask;
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        if (DamageTaken)
        {
            return;
        }

        await OnWorkoutCompleteAsync();
    }
}