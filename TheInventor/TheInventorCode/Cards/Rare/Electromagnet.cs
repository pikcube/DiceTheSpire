using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
    public int BlinkedThisCombat { get; set; }

    protected override IEnumerable<DynamicVar> CanonicalVars => [..MakeCalculatedDamage(4, Bonus, 2)];

    public override Task BeforeCombatStart()
    {
        BlinkedThisCombat = 0;
        return Task.CompletedTask;
    }

    private static decimal Bonus(CardModel arg1, Creature? arg2)
    {
        return arg1 is not Electromagnet e ? 1 : e.BlinkedThisCombat;
    }

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
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

        ++BlinkedThisCombat;
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(3);
        DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}