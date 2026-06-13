using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class Spike() : TheInventorCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Crack);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SpikePower>(3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await SpikePower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<SpikePower>().EnchantedValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<SpikePower>().UpgradeValueBy(2);
    }
}