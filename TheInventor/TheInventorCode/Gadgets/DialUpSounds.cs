using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class DialUpSounds() : GadgetModel(nameof(DialUpSounds))
{
    public override Task AfterCreatureAddedToCombat(Creature creature)
    {
        if (creature.IsPlayer)
        {
            return Task.CompletedTask;
        }

        if (creature.HasPower<MinionPower>())
        {
            CreatureCmd.Stun(creature);
        }

        return Task.CompletedTask;
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power.Owner.IsPlayer)
        {
            return Task.CompletedTask;
        }

        if (power is MinionPower)
        {
            CreatureCmd.Stun(power.Owner);
        }

        return Task.CompletedTask;
    }
}