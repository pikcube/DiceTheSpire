using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class Screwdriver() : TheInventorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, DamageProps.card)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await base.OnPlay(choiceContext, cardPlay);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await Gadget.RechargeAsync(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(BlinkModel.Blink);
    }

    public override string GetScrapId => nameof(DefaultGadget);
}