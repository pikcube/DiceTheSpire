using BaseLib.Extensions;
using DiceTheSpire.Common.Powers;
using DiceTheSpire.Common.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Rare;

public class SuperSet() : TheWarriorCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SuperSetRummagePower>(2M)];
    //PowerVar<SuperSetDrawPower>(1M);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Rummage)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<SuperSetRummagePower>(choiceContext, Owner.Creature, DynamicVars.Power<SuperSetRummagePower>().IntValue, Owner.Creature, this);
        //await PowerCmd.Apply<SuperSetDrawPower>(choiceContext, Owner.Creature, DynamicVars.Power<SuperSetDrawPower>().IntValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<SuperSetRummagePower>().UpgradeValueBy(1);
    }
}