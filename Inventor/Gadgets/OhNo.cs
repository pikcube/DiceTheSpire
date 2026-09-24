using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Gadgets;

public class OhNo() : GadgetModel(nameof(OhNo))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override decimal PowerBase => 5;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner == player && player.Creature.CombatState is not null)
        {
            IReadOnlyList<Creature> enemies = player.Creature.CombatState.HittableEnemies;
            foreach (Creature creature in enemies.ToArray())
            {
                await DoomPower.ApplyAsync(choiceContext, creature, Power, player.Creature, null);
            }
        }
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }
}