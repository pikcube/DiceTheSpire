using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheThief.TheThiefCode.Cards.Common;

public class ToxicOoze() : TheThiefCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(4, ValueProp.Move), new PowerVar<PoisonPower>(2), new RepeatVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this)
            .Targeting(cardPlay.Target).Execute(choiceContext);
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer != Owner.Creature || cardSource != this || result.UnblockedDamage <= 0)
        {
            return;
        }

        await PowerCmd.Apply<PoisonPower>(choiceContext, target, DynamicVars.Poison.BaseValue, Owner.Creature,
            cardSource);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}