using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Common.Listeners;

public interface IAfterCardCountsDownListener
{
    public Task AfterCardCountsDownAsync(IRunState runState, CardModel countdownCard);
}