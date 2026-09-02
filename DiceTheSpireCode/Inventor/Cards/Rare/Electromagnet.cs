using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Keywords;
using Pikcube.Common.Utility;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Cards.Rare;

public class Electromagnet() : TheInventorCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), IOnBlinkListener
{
    public int BlinkedThisCombat { get; set; }

    protected override IEnumerable<DynamicVar> CanonicalVars => [..MakeCalculatedDamage(4, Bonus, 3)];

    private static decimal Bonus(CardModel arg1, Creature? arg2)
    {
        return arg1 is not Electromagnet e ? 1 : e.BlinkedThisCombat;
    }

    public override Task BeforeCombatStart()
    {
        BlinkedThisCombat = 0;
        return Task.CompletedTask;
    }

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    public override string GetScrapId => nameof(Fury);
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