using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards.Token;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;


public class Avalanche() : TheInventorCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Catapult);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AvalanchePower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromCard<Rock>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await AvalanchePower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<AvalanchePower>().EnchantedValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<AvalanchePower>().UpgradeValueBy(1);
    }
}