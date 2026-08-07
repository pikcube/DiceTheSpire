using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;

public interface IAfterBumpListener
{
    public Task AfterBumpAsync(PlayerChoiceContext choiceContext, CardModel card, CardModel? copy);
}