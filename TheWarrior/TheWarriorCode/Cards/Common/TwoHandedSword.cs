using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Common;

public class TwoHandedSword() : TheWarriorCard(4, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), IRangeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8M, DamageProps.card), new PowerVar<FuryPower>(1M), new RepeatVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [BetterStaticHoverTips.RangeHoverTip(this), HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Execute(choiceContext);

        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, 2M, Owner.Creature, this);

    }
    public int MinimumCost => IsUpgraded ? 1 : 2;
    public int MaximumCost => IsUpgraded ? 3 : 4;
    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Damage.UpgradeValueBy(5);
    }

}