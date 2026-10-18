using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class DumbbellQuest : WorkoutQuestCard
{
    //Add 3 common cards to your deck
    public override CardModel RewardCard => Dumbbell.Create();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(CommonsRemaining), 3)];

    [SavedProperty]
    public int CommonsRemaining
    {
        get => DynamicVars[nameof(CommonsRemaining)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(CommonsRemaining)].BaseValue = value;
        }
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Owner || card.Pile?.Type != PileType.Deck || card.Rarity == CardRarity.Common)
        {
            return;
        }

        --CommonsRemaining;
        if (CommonsRemaining > 0)
        {
            return;
        }

        await OnWorkoutComplete();
    }
}