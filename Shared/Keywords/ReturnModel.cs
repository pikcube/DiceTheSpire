using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Keywords;

public class ReturnModel() : CustomSingletonModel(HookType.Combat)
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Return = 0;

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card.Keywords.Contains(Return) && cardLocation.pileType == PileType.Discard)
        {
            return new CardLocation(card.Owner, PileType.Hand, CardPilePosition.Bottom);
        }

        return cardLocation;
    }
}