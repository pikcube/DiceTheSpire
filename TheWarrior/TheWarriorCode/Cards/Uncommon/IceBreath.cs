using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;

public class IceBreath() : TheWarriorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WarriorIceBreathPower>(16M), new PowerVar<FuryPower>(1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WarriorIceBreathPower>(choiceContext, Owner.Creature, DynamicVars.Power<WarriorIceBreathPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WarriorIceBreathPower>().UpgradeValueBy(4);
    }
}