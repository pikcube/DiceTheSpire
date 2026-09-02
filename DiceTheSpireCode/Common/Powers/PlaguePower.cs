using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class PlaguePower : TheInventorPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Applier?.IsPlayer is true && player.Creature != Applier)
        {
            return;
        }

        while (Amount > 0)
        {
            await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, player.RunState, Owner, Applier, null);
            await PowerCmd.Decrement(this);
        }
    }
}