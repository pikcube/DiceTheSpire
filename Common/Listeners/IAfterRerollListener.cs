using DiceTheSpire.Common.Commands;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Listeners;

public interface IAfterRerollListener
{
    public Task AfterRerollAsync(CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration);
}

public interface IAfterFlipListener
{
    public Task AfterFlipAsync(CardModel card, int originalCost, int getAmountToSpend, FlipDuration duration);
}

public interface IAfterNudgeListener
{
    public Task AfterNudgeAsync(CardModel card, int originalCost, int getAmountToSpend, NudgeDuration duration);
}