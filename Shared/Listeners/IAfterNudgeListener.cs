using DiceTheSpire.Shared.Commands;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IAfterNudgeListener
{
    public Task AfterNudgeAsync(CardModel card, int originalCost, int getAmountToSpend, NudgeDuration duration);
}