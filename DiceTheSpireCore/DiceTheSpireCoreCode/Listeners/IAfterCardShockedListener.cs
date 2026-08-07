using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;

public interface IAfterCardShockedListener
{
    public Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, ShockPower shock, CardModel card);
}