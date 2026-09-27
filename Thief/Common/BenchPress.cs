using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Thief.Common;

public class BenchPress() : TheThiefCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), ICountdown
{
    public int MaxCount
    {
        get;
        set;
    } = 2;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7M, ValueProp.Move), new PowerVar<StrengthPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target).FromCard(this, cardPlay)
            .Execute(choiceContext);
        await PowerCmd.Apply<BenchPressPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Strength.UpgradeValueBy(1);
    }
}