using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

public class Blowtorch() : GadgetModel(nameof(Blowtorch))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override decimal PowerBase => 7;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner == player && player.Creature.CombatState is not null)
        {
            Parent.Flash();
            await CreatureCmd.Damage(choiceContext, player.Creature.CombatState.Enemies, Power,
                DamageProps.nonCardUnpowered, player.Creature, null, null);
        }
    }
}