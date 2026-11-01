using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Thief.Rare;

public class SkeletonKey() : TheThiefCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override bool HasEnergyCostX => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.FromKeyword(CardKeyword.Sly)
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        IEnumerable<CardModel> cards = DiceyHelperFunctions.GetDistinctOfClassForCombat(Owner,
            DiceyHelperFunctions.NonThiefCharacterPools,
            ResolveEnergyXValue(),
            Owner.RunState.Rng.CombatCardGeneration);
        IEnumerable<CardModel> cardModels = cards as CardModel[] ?? [.. cards];
        foreach (CardModel card in cardModels)
        {
            card.AddKeyword(CardKeyword.Retain);
            card.AddKeyword(CardKeyword.Sly);
            card.AddKeyword(CardKeyword.Exhaust);
        }

        await CardPileCmd.AddGeneratedCardsToCombat(cardModels, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}