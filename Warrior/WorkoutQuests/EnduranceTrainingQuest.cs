using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class EnduranceTrainingQuest : WorkoutQuestCard
{
    //Climb 8 floors
    public override CardModel RewardCard => EnduranceTraining.Create();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(FloorsRemaining), 8)];

    [SavedProperty]
    public int FloorsRemaining
    {
        get => DynamicVars[nameof(FloorsRemaining)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(FloorsRemaining)].BaseValue = value;
        }
    }

    [SavedProperty] public int LastFloor { get; set; } = -1;

    public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card == this)
        {
            LastFloor = Owner.RunState.TotalFloor;
        }

        return Task.CompletedTask;
    }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (Owner.RunState.TotalFloor > LastFloor)
        {
            --FloorsRemaining;
            LastFloor = Owner.RunState.TotalFloor;
        }

        if (FloorsRemaining > 0)
        {
            return;
        }

        await OnWorkoutCompleteAsync();
    }
}