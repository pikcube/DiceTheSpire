using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Uncommon;

public class FrostHammer() : TheInventorCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(WallOfIce);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, DamageProps.card), new PowerVar<VulnerablePower>(1)];


    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<FreezePower>(), HoverTipFactory.FromPower<VulnerablePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await FreezePower.ApplyAsync(choiceContext, cardPlay.Target, 1, Owner.Creature, this);

        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.bluntPath)
            .Execute(choiceContext);

        for (int n = 0; n < DynamicVars.Vulnerable.IntValue; ++n)
        {
            await NextTurnVulnerablePower.ApplyAsync(choiceContext, cardPlay.Target, 1, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}