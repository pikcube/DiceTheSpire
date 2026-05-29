using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class Bonk() : GadgetModel(nameof(Bonk))
{
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Creature? target = player.Creature.CombatState?.Enemies
            .TakeRandom(1, player.RunState.Rng.CombatTargets)
            .SingleOrDefault();

        if (target is null)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, target, 5, DamageProps.nonCardUnpowered, null, null);
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }
}