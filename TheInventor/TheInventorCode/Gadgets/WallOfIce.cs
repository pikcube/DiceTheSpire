using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

public class WallOfIce() : AbstractGadget(nameof(WallOfIce))
{ 
    public override Task BeforeCombatStart()
    {
        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player || player.Creature.CombatState is null || player.Creature.CombatState.RoundNumber <= 1)
        {
            return Task.CompletedTask;
        }

        BreakMe();

        return Task.CompletedTask;
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (Parent?.Owner.Creature != target)
        {
            return 1;
        }

        return 0.5M;
    }
}