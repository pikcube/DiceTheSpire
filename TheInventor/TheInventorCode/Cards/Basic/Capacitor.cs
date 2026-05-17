using DiceTheSpireCore.DiceTheSpireCoreCode.Singletons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Basic;

public class Capacitor() : TheInventorCard(-1, CardType.Attack, CardRarity.Basic, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, DamageProps.card), new PowerVar<VulnerablePower>(2)];

    public override string OnScrap() => nameof(ShortCircuit);

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState is null)
        {
            return;
        }

        foreach (Creature c in CombatState.Enemies)
        {
            await CreatureCmd.Damage(choiceContext, c, DynamicVars.Damage, this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, c, DynamicVars.Vulnerable.IntValue, Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}