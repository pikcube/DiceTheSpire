using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheInventor.TheInventorCode.Keywords;

[UsedImplicitly]
public class ScrapKeyword() : CustomSingletonModel(false, false)
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Scrap = 0;
}