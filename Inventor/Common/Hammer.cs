using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Common;

public class Hammer() : TheInventorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(7, DamageProps.card), new PowerVar<VulnerablePower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .WithValueProp(DynamicVars.Damage.Props)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.bluntPath)
            .Execute(choiceContext);

        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.IntValue,
            Owner.Creature, cardPlay.Card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }

    //Character specific BS, please ignore
    public override string GetScrapId => nameof(ShortCircuit);
}