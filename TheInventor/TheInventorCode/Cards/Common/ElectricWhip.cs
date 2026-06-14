using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Common;

public class ElectricWhip() : TheInventorCard(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(ShortCircuit);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, DamageProps.card), new PowerVar<ShockPower>(2)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<ShockPower>(DynamicVars.Power<ShockPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //ArgumentNullException.ThrowIfNull(cardPlay.Target);

        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await PowerCmd.Apply<ShockPower>(choiceContext, Owner.Creature, DynamicVars.Power<ShockPower>().IntValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ShockPower>().UpgradeValueBy(-1);
    }
}