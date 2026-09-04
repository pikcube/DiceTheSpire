using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IAfterCardShockedListener
{
    public Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, ShockPower shock, CardModel card);
}