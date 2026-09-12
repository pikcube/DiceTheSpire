using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Rare;

public class Electromagnet() : TheInventorCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), IOnShockListener
{
    public int ShockedThisCombat { get; set; }

    protected override IEnumerable<DynamicVar> CanonicalVars => [..MakeCalculatedDamage(4, Bonus, 3)];

    private static decimal Bonus(CardModel arg1, Creature? arg2)
    {
        return arg1 is not Electromagnet e ? 1 : e.ShockedThisCombat;
    }

    public override Task BeforeCombatStart()
    {
        ShockedThisCombat = 0;
        return Task.CompletedTask;
    }

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

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
    public Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        ++ShockedThisCombat;
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(3);
        DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}