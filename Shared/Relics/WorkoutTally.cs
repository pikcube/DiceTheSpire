using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Relics;


[UsedImplicitly]
public class WorkoutTally : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public override bool HasUponPickupEffect => base.HasUponPickupEffect;

    public override async Task AfterObtained()
    {
        //List<Reward> rewards = new List<Reward>();
        //PredeterminedCardReward rewards = new PredeterminedCardReward WorkoutFactory.CreateWorkoutReward(CardCreationSource.Other, Owner, 3);
        ////CardCreationOptions options = CardCreationOptions WorkoutFactory.CreateWorkoutReward(CardCreationSource.Other, Owner, 3);
        ////new PredeterminedCardReward WorkoutFactory.CreateWorkoutReward(CardCreationSource.Other, Owner, 3));
        ////IReadOnlyList <IWorkoutQuest>
        ////WorkoutFactory.CreateWorkoutReward(CardCreationSource.Other, Owner, 3);
        //for (int index = 0; index < 1; ++index)
        //    rewards.Add((Reward)new WorkoutFactory.CreateWorkoutReward(CardCreationSource.Other, Owner, 3)
        //await RewardsCmd.OfferCustom(Owner, rewards);
        Flash();

    }

}