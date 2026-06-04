using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Gadgets;

public class Overload() : GadgetModel(nameof(Overload))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Parent?.Owner || player.Creature.CombatState is null)
        {
            return;
        }

        Creature[] creatures = [.. player.Creature.CombatState.Creatures];

        foreach (Creature creature in creatures)
        {
            PowerModel[] powers = [..creature.Powers
                .Where(p =>
                    p.StackType == PowerStackType.Counter
                )
            ];

            foreach (PowerModel power in powers)
            {
                await PowerCmd.ModifyAmount(choiceContext, power, power.Amount, null, null);
            }
        }
    }
}