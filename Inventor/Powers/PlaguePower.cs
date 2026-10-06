using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Inventor.Powers;

public class PlaguePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Applier?.IsPlayer is true && player.Creature != Applier)
        {
            return;
        }

        if (Amount == 1)
        {
            await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, player.RunState, Owner, Applier, null);
            await PowerCmd.Remove(this);
        }
        else
        {
            await PowerCmd.Decrement(this);
        }
    }
}