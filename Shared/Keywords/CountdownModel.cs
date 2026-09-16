using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DiceTheSpire.Shared.Keywords;

public class CountdownModel() : CustomSingletonModel(HookType.Combat)
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before), UsedImplicitly]
    public static CardKeyword Countdown = 0;
}