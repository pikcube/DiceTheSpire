using MegaCrit.Sts2.Core.Models;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;

public class CartwheelQuest : WorkoutQuestCard
{
    //Add 5 cards to your deck
    public override CardModel RewardCard => Cartwheel.Create();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(CardsRemaining), 5)];

    [SavedProperty]
    public int CardsRemaining
    {
        get => DynamicVars[nameof(CardsRemaining)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(CardsRemaining)].BaseValue = value;
        }
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Owner || card.Pile?.Type != PileType.Deck)
        {
            return;
        }

        --CardsRemaining;
        if (CardsRemaining > 0)
        {
            return;
        }

        await OnWorkoutCompleteAsync();
    }
}