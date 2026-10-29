using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IAfterWorkoutCompletedListener
{
    public Task AfterWorkoutCompleteAsync(CardModel card);
}