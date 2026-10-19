using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class DeepFreezeQuest : WorkoutQuestCard
{
    //Skip 2 card rewards
    public override CardModel RewardCard => DeepFreeze.Create();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(RewardsLeft), 2)];

    [SavedProperty]
    public int RewardsLeft
    {
        get => DynamicVars[nameof(RewardsLeft)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(RewardsLeft)].BaseValue = value;
        }
    }

    public override bool TryModifyCardRewardAlternatives(Player player, CardReward cardReward,
        List<CardRewardAlternative> alternatives)
    {
        if (Owner != player)
        {
            return false;
        }

        alternatives.Add(new CardRewardAlternative($"{MainFile.ModId}_{nameof(DeepFreezeQuest)}", AfterSkip, PostAlternateCardRewardAction.EndSelectionAndCompleteReward));
        return true;
    }

    public async Task AfterSkip()
    {
        --RewardsLeft;
        if (RewardsLeft > 0)
        {
            return;
        }

        await OnWorkoutCompleteAsync();
    }
}