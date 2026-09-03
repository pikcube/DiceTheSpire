using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Basic;

public class DiceyCapacitor() : TheInventorCard(-1, CardType.Attack, CardRarity.Basic, TargetType.RandomEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, DamageProps.cardUnpowered)];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        for (int n = 0; n < (IsUpgraded ? 3 : 2); ++n)
        {
            if (CombatState is null || RunState is null)
            {
                return;
            }
            await CreatureCmd.Damage(choiceContext, CombatState.Enemies.TakeRandom(1, RunState.Rng.CombatTargets),
                DynamicVars.Damage, Owner.Creature, this, null);
        }
    }

    public override string GetScrapId => nameof(ShortCircuit);
}