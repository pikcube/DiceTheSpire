using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Powers;

public class FeedbackLoopPower : DiceTheSpirePower, IOnShockListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner.Creature != Owner)
        {
            return;
        }
        await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
    }
}