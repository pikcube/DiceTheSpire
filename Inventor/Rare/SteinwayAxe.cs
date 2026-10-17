using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Rare;


public class SteinwayAxe() : TheInventorCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, DamageProps.card)];
    public override string GetScrapId => nameof(Blowtorch);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        AttackCommand results = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        int damage = results.Results.SingleOrDefault()?.SingleOrDefault()?.TotalDamage ?? 0;

        await NextTurnDamagePower.ApplyAsync(choiceContext, cardPlay.Target, damage, 1, Owner.Creature, this);
        await NextTurnDamagePower.ApplyAsync(choiceContext, cardPlay.Target, damage, 2, Owner.Creature, this);
        await NextTurnDamagePower.ApplyAsync(choiceContext, cardPlay.Target, damage, 3, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}