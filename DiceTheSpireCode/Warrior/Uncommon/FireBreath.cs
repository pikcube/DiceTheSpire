using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.DiceTheSpireCode.Warrior.Uncommon;
public class FireBreath() : TheWarriorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WarriorFireBreathPower>(6M), new PowerVar<FuryPower>(1M), new PowerVar<VigorPower>(4M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue), HoverTipFactory.FromPower<VigorPower>(DynamicVars.Power<VigorPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WarriorFireBreathPower>(choiceContext, Owner.Creature, DynamicVars.Power<WarriorFireBreathPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WarriorFireBreathPower>().UpgradeValueBy(2);
        DynamicVars.Power<VigorPower>().UpgradeValueBy(2);
    }
}