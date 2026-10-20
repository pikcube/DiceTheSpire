using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class LightningKickQuest : WorkoutQuestCard
{
    //Clear 5 combats
    public override CardModel RewardCard => LightningKick.Create();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(CombatsRemaining), 5)];

    [SavedProperty]
    public int CombatsRemaining
    {
        get => DynamicVars[nameof(CombatsRemaining)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(CombatsRemaining)].BaseValue = value;
        }
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        --CombatsRemaining;
        if (CombatsRemaining > 0)
        {
            await OnWorkoutCompleteAsync();
        }
    }
}