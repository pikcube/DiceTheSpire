using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Inventor.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Rare;

public class Grindstone() : TheInventorCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(AutoBump);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GrindstonePower>(1)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await GrindstonePower.ApplyAsync(choiceContext, Owner, DynamicVars.Power<GrindstonePower>().IntValue, Owner, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<GrindstonePower>().UpgradeValueBy(1);
    }
}