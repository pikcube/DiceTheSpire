using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IAfterCardShockedListener
{
    public Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, CardModel card);
}