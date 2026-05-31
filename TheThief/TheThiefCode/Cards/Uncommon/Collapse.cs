using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheThief.TheThiefCode.Cards.Uncommon;

[Pool(typeof(TokenCardPool))]
public class Collapse() : TheThiefCard(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12M, ValueProp.Move)];

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4M);
    }
}