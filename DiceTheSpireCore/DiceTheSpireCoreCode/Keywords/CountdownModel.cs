using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;

public class CountdownModel() : CustomSingletonModel(HookType.Combat)
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Countdown = 0;
}