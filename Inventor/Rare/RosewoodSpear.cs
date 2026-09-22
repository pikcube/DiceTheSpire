using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Rare;


public class RosewoodSpear() : TheInventorCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, DamageProps.card)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ShockModel.Shock];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<ThornsPower>()];
    public override string GetScrapId => nameof(Needle);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        AttackCommand cmd = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        int total = cmd.Results.SelectMany(resultList => resultList).Select(result => result.UnblockedDamage).Sum();

        await ThornsPower.ApplyAsync(choiceContext, Owner.Creature, total, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}