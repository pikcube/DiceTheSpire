using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Extensions;
using Pikcube.Common.Powers;

namespace DiceTheSpire.Shared.Powers;
public class NextTurnFuryPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner != player.Creature)
        {
            return;
        }

        await FuryPower.ApplyAsync(choiceContext, Owner, Amount, Owner, null);
        await PowerCmd.Remove(this);
    }
}