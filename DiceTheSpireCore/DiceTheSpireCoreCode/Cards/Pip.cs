using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Cards;

[Pool(typeof(StatusCardPool))]
public class Pip() : DiceTheSpireCoreCard(1, CardType.Status, CardRarity.Status, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Sly, CardKeyword.Exhaust];
}