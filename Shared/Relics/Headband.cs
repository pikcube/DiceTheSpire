using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using DiceTheSpire.Shared.Listeners;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class Headband : TheWarriorRelic, IAfterWorkoutCompletedListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(10M)];

    public async Task AfterWorkoutCompleteAsync(CardModel card)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.IntValue);
        Flash();
    }

}