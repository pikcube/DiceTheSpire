using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Rare;

public class BlastChiller() : TheInventorCard(-1, CardType.Attack, CardRarity.Rare, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0), 
        new ExtraDamageVar(1), 
        new CalculatedDamageVar(DamageProps.cardUnpowered).WithMultiplier((model, _) => model.Owner.Creature.Block)
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
            await CreatureCmd.Damage(choiceContext, [.. CombatState.Enemies], Owner.Creature.Block,
                DamageProps.cardUnpowered, Owner.Creature, this, null);
        }
        else
        {
            await CreatureCmd.Damage(choiceContext, CombatState.Enemies.TakeRandom(1, RunState.Rng.CombatTargets), Owner.Creature.Block,
                DamageProps.cardUnpowered, Owner.Creature, this, null);
        }
    }

    public override string GetScrapId => nameof(WallOfIce);
}