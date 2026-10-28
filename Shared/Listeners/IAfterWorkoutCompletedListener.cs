using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IAfterWorkoutCompletedListener
{
    public Task OnWorkoutCompleteAsync(PlayerChoiceContext choiceContext, CardModel card);
}