using MegaCrit.Sts2.Core.Entities.Cards;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

  
public class FocusForm() : TheInventorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(BattleWrench);
}