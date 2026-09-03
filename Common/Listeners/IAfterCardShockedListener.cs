using DiceTheSpire.Common.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Listeners;

public interface IAfterCardShockedListener
{
    public Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, ShockPower shock, CardModel card);
}