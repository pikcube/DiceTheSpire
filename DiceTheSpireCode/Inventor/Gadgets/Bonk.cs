using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

[UsedImplicitly]
public class Bonk() : GadgetModel(nameof(Bonk))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override decimal PowerBase => 5;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player)
        {
            return;
        }

        Creature? target = player.Creature.CombatState?.Enemies
            .TakeRandom(1, player.RunState.Rng.CombatTargets)
            .SingleOrDefault();

        if (target is null)
        {
            return;
        }

        Parent.Flash();
        await CreatureCmd.Damage(choiceContext, target, Power, DamageProps.nonCardUnpowered, null, null);
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }
}