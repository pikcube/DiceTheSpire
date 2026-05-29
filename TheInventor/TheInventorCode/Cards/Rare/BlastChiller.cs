using DiceTheSpireCore.DiceTheSpireCoreCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class BlastChiller() : TheInventorCard(-1, CardType.Attack, CardRarity.Rare, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0), 
        new ExtraDamageVar(1), 
        new CalculatedDamageVar(DamageProps.card).WithMultiplier((model, creature) => model.Owner.Creature.Block)
    ];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (RunState is null || CombatState is null)
        {
            return;
        }

        if (IsUpgraded)
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);
        }
        else
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(this)
                .WithHitCount(1)
                .TargetingRandomOpponents(CombatState)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);
        }
        await DiceyHooks.OnTurnEndInHand(this, RunState, CombatState);
    }

    public override string GetScrapId => nameof(WallOfIce);
}