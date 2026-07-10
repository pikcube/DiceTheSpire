using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IAfterCardCountsDownListener
{
    public Task AfterCardCountsDownAsync(IRunState runState, CardModel countdownCard);
}