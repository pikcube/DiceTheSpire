using BaseLib.Utils;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior;

[Pool(typeof(QuestCardPool))]
public abstract class WorkoutQuestCard() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public abstract CardModel RewardCard { get; }

    public override int MaxUpgradeLevel => 0;

    protected sealed override IEnumerable<IHoverTip> ExtraHoverTips =>
        [.. ExtraWorkoutHoverTips, HoverTipFactory.FromCard(RewardCard)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected virtual IEnumerable<IHoverTip> ExtraWorkoutHoverTips => [];

    public async Task OnWorkoutComplete()
    {
        if (DeckVersion is not null)
        {
            return;
        }

        if (Pile?.Type != PileType.Deck)
        {
            return;
        }

        await CardCmd.Transform(this, RewardCard.CreateNewInstance(Owner), CardPreviewStyle.GridLayout);
    }
}