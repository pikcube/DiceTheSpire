using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Keywords;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class Electromagnet() : TheInventorCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), IOnBlinkListener
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, DamageProps.card), new ExtraDamageVar(2)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    public override string GetScrapId => nameof(MagicDice);
    public Task AfterCardBlinkedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        DynamicVars.Damage.UpgradeValueBy(DynamicVars.ExtraDamage.IntValue);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}