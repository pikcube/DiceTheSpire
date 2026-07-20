using MegaCrit.Sts2.Core.Entities.Cards;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class PlasmaCannon() : TheInventorCard(3, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(BigBomb);
}