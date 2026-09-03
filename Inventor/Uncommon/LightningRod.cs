using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Uncommon;

public class LightningRod() : TheInventorCard(4, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(PowerUp);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(0, DamageProps.card)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override Task BeforeCombatStart()
    {
        DynamicVars.Damage.BaseValue = 0;
        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner.Creature)
        {
            DynamicVars.Damage.UpgradeValueBy(result.TotalDamage);
        }

        return Task.CompletedTask;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }


    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}