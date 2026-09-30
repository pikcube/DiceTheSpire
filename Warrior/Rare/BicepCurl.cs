using BaseLib.Extensions;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Rare;

public class BicepCurl() : TheWarriorCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    //public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BicepCurlPower>(2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge), HoverTipFactory.Static(BetterStaticHoverTips.Bump)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BicepCurlPower>(choiceContext, Owner.Creature, DynamicVars.Power<BicepCurlPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<PermafrostRetentionPower>().UpgradeValueBy(1);
    }
}