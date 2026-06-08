using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Utility;

namespace TheInventor.TheInventorCode.Powers;

public class FeedbackLoopPower : TheInventorPower, IOnBlinkListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private int CardsBlinkedThisTurn { get; set; }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        CardsBlinkedThisTurn = 0;
        return Task.CompletedTask;
    }

    public async Task AfterCardBlinkedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (Owner != card.Owner.Creature)
        {
            return;
        }

        if (CardsBlinkedThisTurn < Amount)
        {
            await CardPileCmd.Draw(choiceContext, card.Owner);
        }

        CardsBlinkedThisTurn++;
    }
}