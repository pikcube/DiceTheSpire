using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

public class Bonk() : AbstractGadget(nameof(Bonk))
{
    public override string GadgetText => $"Bonk: At the start of each turn, deal [blue]5[/blue] damage to a random enemy.";
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState is null)
        {
            return;
        }

        Creature target = player.Creature.CombatState.Creatures
            .TakeRandom(1, player.RunState.Rng.CombatTargets)
            .Single();

        await CreatureCmd.Damage(choiceContext, target, 5, DamageProps.nonCardUnpowered, null, null);
    }
}