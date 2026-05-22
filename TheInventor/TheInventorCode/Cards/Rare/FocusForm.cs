using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Keywords;

namespace TheInventor.TheInventorCode.Cards.Rare;

  
public class FocusForm() : TheInventorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ScrapKeyword.Scrap];

    public override string GetScrapId => nameof(BattleWrench);

    protected override void OnUpgrade()
    {
        RemoveKeyword(ScrapKeyword.Scrap);
    }
}