using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class ShortCircuit() : GadgetModel(nameof(ShortCircuit))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override bool IsAllowedAsTempGadget => false;

    public override decimal PowerBase => 5;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Parent?.Owner || player.Creature.CombatState is null)
        {
            return;
        }

        for (int n = 0; n < Power; ++n)
        {
            foreach (Creature c in player.Creature.CombatState.Enemies)
            {
                await PowerCmd.Apply<VulnerablePower>(choiceContext, c, 1, null, null);
            }
        }

        BreakMe();
    }
}