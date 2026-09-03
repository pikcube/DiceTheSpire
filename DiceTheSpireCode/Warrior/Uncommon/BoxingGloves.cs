using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.DynamicVars;
using DiceTheSpire.DiceTheSpireCode.Common.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Warrior.Uncommon;

[UsedImplicitly]
public class BoxingGloves() : TheWarriorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    //public override TargetType TargetType => EnergyCost.GetAmountToSpend() > 0 ? TargetType.AnyEnemy : TargetType.Self;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, DamageProps.card), new PowerVar<ReducePower>(1), .. RangeVars.Make(0, 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ReducePower>(DynamicVars.Power<ReducePower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        int xValue = EnergyCost.GetAmountToSpend();

        if (xValue != 0)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);
        }
        else
        {

            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);

            await PowerCmd.Apply<ReducePower>(choiceContext, Owner.Creature, DynamicVars.Power<ReducePower>().IntValue, Owner.Creature, this);
        }
    
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.MaxRange.UpgradeValueBy(-1);
    }

}