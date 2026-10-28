using DiceTheSpire.Shared.Interfaces;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class Headband : TheWarriorRelic, IAfterWorkoutCompletedListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(10M)];

    public async Task OnWorkoutCompleteAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.IntValue, true);
        Flash();
    }

}