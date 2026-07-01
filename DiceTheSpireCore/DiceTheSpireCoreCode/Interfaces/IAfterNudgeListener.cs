using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IAfterNudgeListener
{
    public Task AfterNudgeAsync(PlayerChoiceContext choiceContext, CardModel card, bool wasExhausted);
}