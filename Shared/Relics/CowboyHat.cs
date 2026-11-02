using DiceTheSpire.Shared.Commands;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class CowboyHat : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3)];
    public async Task AfterRerollAsync(PlayerChoiceContext choiceContext, CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
    {
        if (!CombatManager.Instance.IsInProgress || Owner.Creature.CombatState is null)
        {
            return;
        }

        IReadOnlyList<Creature> hittableEnemies = Owner.Creature.CombatState.HittableEnemies;
        Creature? item = Owner.Creature.Player?.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
        if (hittableEnemies.Count == 0 || Owner.Creature.Player is null || item is null)
        {
            return;
        }
        if (card.EnergyCost.GetAmountToSpend() < 3)
        {
            return;
        }
        await CreatureCmd.Damage(choiceContext, item, 10, ValueProp.Unpowered | ValueProp.Unpowered, null, null, null);
    }

}