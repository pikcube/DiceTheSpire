using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;

public class ShockedModel() : CustomSingletonModel(HookType.Combat)
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Shocked = 0;
}