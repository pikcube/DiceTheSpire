using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Shared.Powers;

public class BeeStingPower : TheInventorPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner == player.Creature)
        {
            await InventorHelperFunctions.ShockRandomAsync(choiceContext, player, player.RunState.Rng.CombatCardSelection, Amount);
        }
    }
}