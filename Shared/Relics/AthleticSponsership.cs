using DiceTheSpire.Shared.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class AthleticSponsership : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (room?.RoomType != RoomType.Elite || player == Owner)
        {
            return false;
        }

        rewards.Add(WorkoutFactory.CreateWorkoutReward(CardCreationSource.Encounter, Owner, 2));
        return true;

    }

    public override Task AfterModifyingRewards()
    {
        Flash();
        return Task.CompletedTask;
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<CombatSpatula>();
    }
}