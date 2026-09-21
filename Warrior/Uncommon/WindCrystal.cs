using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Uncommon;

public class WindCrystal() : TheWarriorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self), ICrystalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WindCrystalPower>(1M)];
    //PowerVar<SuperSetDrawPower>(1M);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Rummage)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WindCrystalPower>(choiceContext, Owner.Creature, DynamicVars.Power<WindCrystalPower>().IntValue, Owner.Creature, this);
        //await PowerCmd.Apply<SuperSetDrawPower>(choiceContext, Owner.Creature, DynamicVars.Power<SuperSetDrawPower>().IntValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<WindCrystalPower>().UpgradeValueBy(1);
    }
}