using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class DoubleEdge() : TheInventorCard(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(PowerUp);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(35, DamageProps.card), new PowerVar<StrengthPower>(2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await StrengthPower.ApplyAsync(choiceContext, Owner.Creature, -DynamicVars.Strength.EnchantedValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(-1);
    }
}