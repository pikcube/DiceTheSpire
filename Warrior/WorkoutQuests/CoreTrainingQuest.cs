using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;

public class CoreTrainingQuest : WorkoutQuestCard
{
    //Add 4 skills to your deck
    public override CardModel RewardCard => CoreTraining.Create();
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(SkillsRemaining), 4)];

    [SavedProperty]
    public int SkillsRemaining
    {
        get => DynamicVars[nameof(SkillsRemaining)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(SkillsRemaining)].BaseValue = value;
        }
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Owner || card.Pile?.Type != PileType.Deck || card.Type != CardType.Skill)
        {
            return;
        }

        --SkillsRemaining;
        if (SkillsRemaining > 0)
        {
            return;
        }

        await OnWorkoutComplete();
    }
}