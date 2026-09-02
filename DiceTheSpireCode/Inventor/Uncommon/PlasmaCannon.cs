using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpire.DiceTheSpireCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;

public class PlasmaCannon() : TheInventorCard(3, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(PowerUp);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlasmaCannonPower>(10)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<GrindstonePower>(DynamicVars.Power<PlasmaCannonPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlasmaCannonPower.ApplyAsync(choiceContext, Owner, DynamicVars.Power<PlasmaCannonPower>().IntValue, Owner, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<PlasmaCannonPower>().UpgradeValueBy(5);
    }
}