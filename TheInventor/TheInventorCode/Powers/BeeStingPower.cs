using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Powers;

public class BeeStingPower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player == Owner.Player ? amount + Amount : amount;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner == player.Creature)
        {
            await ShockPower.ApplyAsync(choiceContext, Owner, 1, Owner, null);
        }
    }
}