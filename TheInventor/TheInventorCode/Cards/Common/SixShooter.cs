using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Commands;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Common;

public class SixShooter() : TheInventorCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, DamageProps.card), new("Hits", 3), new PowerVar<StrengthPower>(3)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars["Hits"].IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        StrengthPower? result = await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, -DynamicVars.Power<StrengthPower>().IntValue, Owner.Creature, this);
        if (result is null)
        {
            return;
        }

        await JinxCmd.JinxAsync(choiceContext, Owner.Creature, 1, true, Description, ReturnStrength, Owner.Creature, this);
    }

    private async Task ReturnStrength(PlayerChoiceContext choiceContext, Creature target)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Power<StrengthPower>().IntValue, Owner.Creature, this);
    }

    public override string GetScrapId => nameof(Crack);

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}