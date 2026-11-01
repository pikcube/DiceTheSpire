using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Rare;

public class Electromagnet() : TheInventorCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), ISimpleCalculatedDamageCard
{
    public int BaseDamage => 4;
    public int ExtraDamageMultiplier => 3;
    public ValueProp DamageType => DamageProps.card;

    public decimal CalculateDamage(Creature? target)
    {
        return CombatManager.Instance.History.Entries.OfType<CardShockedEntry>().Count(cse => cse.Card.Owner == Owner);
    }

    public override Task BeforeCombatStart()
    {
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

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(3);
        DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}