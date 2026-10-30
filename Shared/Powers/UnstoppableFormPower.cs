using DiceTheSpire.Shared.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Shared.Powers;


public class UnstoppableFormPower : DiceTheSpirePower
{

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player) 
    {
        if (Owner.Player is null || Owner.Player != player)
        {
            return;
        }

        await DodgeCmd.DodgeAsync(choiceContext, Owner.Player, Amount, this);
    }
}