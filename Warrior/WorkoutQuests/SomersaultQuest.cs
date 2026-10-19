using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class SomersaultQuest : WorkoutQuestCard
{
    //Enter 4 ? rooms
    public override CardModel RewardCard => Somersault.Create();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(RoomsLeft), 4)];

    [SavedProperty]
    public int RoomsLeft
    {
        get => DynamicVars[nameof(RoomsLeft)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(RoomsLeft)].BaseValue = value;
        }
    }


    public override async Task BeforeRoomEntered(AbstractRoom room)
    {
        MapPoint? currentMapPoint = Owner.RunState.CurrentMapPoint;
        if (currentMapPoint is null)
        {
            return;
        }

        if (currentMapPoint.PointType != MapPointType.Unknown)
        {
            return;
        }

        --RoomsLeft;

        if (RoomsLeft > 0)
        {
            return;
        }

        await OnWorkoutCompleteAsync();
    }
}