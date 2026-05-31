using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInventor.TheInventorCode.Gadgets;

public class Needle() : GadgetModel(nameof(Needle))
{
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner is null)
        {
            return;
        }

        await PowerCmd.Apply<ThornsPower>(choiceContext, Parent.Owner.Creature, 1, Parent.Owner.Creature, null);
    }
}