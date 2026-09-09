using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Uncommon;


public class SledgeHammer() : TheInventorCard(-1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, DamageProps.cardUnpowered)];

    public override bool HasTurnEndInHandEffect => true;

    public override string GetScrapId => nameof(ShortCircuit);

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState is null || CombatState.Enemies.Count == 0)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, CombatState.Enemies, DynamicVars.Damage, Owner.Creature, this, null);
        DynamicVars.Damage.UpgradeValueBy(-1);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}