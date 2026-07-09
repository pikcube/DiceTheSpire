using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;

public class ReturningModel() : CustomSingletonModel(HookType.Combat)
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Returning;

    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay,
        ResourceInfo resources, PileType pileType, CardPilePosition position)
    {
        return (card.Keywords.Contains(Returning) && pileType == PileType.Discard) ? (PileType.Hand, CardPilePosition.Bottom) : (pileType, position);
    }
}