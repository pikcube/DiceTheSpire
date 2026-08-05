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
public class FuriousForm() : TheWarriorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self), IRangeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuriousFormPower>(1), new PowerVar<FuryPower>(1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [BetterStaticHoverTips.RangeHoverTip(this), HoverTipFactory.FromPower<FuriousFormPower>(DynamicVars.Power<FuriousFormPower>().IntValue), HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FuriousFormPower>(choiceContext, Owner.Creature, DynamicVars.Power<FuriousFormPower>().IntValue, Owner.Creature, this);
    }

    public int MinimumCost => IsUpgraded ? 0 : 3;
    public int MaximumCost => 3;
}