using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class PiledriverQuest : WorkoutQuestCard
{
    //Add 4 attacks to your deck
    public override CardModel RewardCard => Piledriver.Create();
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(AttacksRemaining), 4)];

    [SavedProperty]
    public int AttacksRemaining
    {
        get => DynamicVars[nameof(AttacksRemaining)].IntValue;
        set
        {
            AssertMutable();
            DynamicVars[nameof(AttacksRemaining)].BaseValue = value;
        }
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Owner || card.Pile?.Type != PileType.Deck || card.Type != CardType.Attack)
        {
            return;
        }

        --AttacksRemaining;
        if (AttacksRemaining > 0)
        {
            return;
        }

        await OnWorkoutCompleteAsync();
    }
}