using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWarrior.TheWarriorCode.Cards.Rare;
public class Shriek() : TheWarriorCard(2, CardType.Power, CardRarity.Rare, TargetType.Self), IRangeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WarriorShriekPower>(1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [BetterStaticHoverTips.RangeHoverTip(this), HoverTipFactory.FromPower<WarriorShriekPower>(DynamicVars.Power<WarriorShriekPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WarriorShriekPower>(choiceContext, Owner.Creature, DynamicVars.Power<WarriorShriekPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WarriorShriekPower>().UpgradeValueBy(1);
    }
    public int MinimumCost => 0;
    public int MaximumCost => 2;
}

