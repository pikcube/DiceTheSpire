using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Relics;


public class PortableFan : TheInventorRelic
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator != Owner || Owner.Creature.CombatState is null)
        {
            return;
        }
        foreach (Creature c in Owner.Creature.CombatState.Enemies)
        {
            HookPlayerChoiceContext context = new(Owner, LocalContext.NetId ?? 0, GameActionType.Combat);
            await InventorHelperFunctions.ApplyRandomDebuffAsync(context, Owner.RunState, c, Owner.Creature, null);
        }
    }
}