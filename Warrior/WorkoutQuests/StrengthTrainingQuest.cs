using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class StrengthTrainingQuest : WorkoutQuestCard
{
    //Add 3 powers to your deck
    public override CardModel RewardCard => StrengthTraining.Create();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(PowersRemaining), 3)];

    [SavedProperty]
    public int PowersRemaining
    {
        get => DynamicVars[nameof(PowersRemaining)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(PowersRemaining)].BaseValue = value;
        }
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Owner || card.Pile?.Type != PileType.Deck || card.Type == CardType.Power)
        {
            return;
        }

        --PowersRemaining;
        if (PowersRemaining > 0)
        {
            return;
        }

        await OnWorkoutCompleteAsync();
    }
}