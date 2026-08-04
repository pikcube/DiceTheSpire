using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Common;

public class BattleAxe() : TheWarriorCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), IRangeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, DamageProps.card), new RepeatVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [BetterStaticHoverTips.RangeHoverTip(this)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        int extraHits = Owner.Creature.GetPower<AxeMasteryPower>()?.Amount ?? 0;

        int hitCount = 2 + extraHits;

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .WithHitCount(hitCount)
            .FromCard(this, cardPlay)
            .WithHitFx(VfxCmd.slashPath)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await PowerCmd.Apply<AxeMasteryPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
    public int MinimumCost => 0;
    public int MaximumCost => IsUpgraded ? 1 : 2;

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}