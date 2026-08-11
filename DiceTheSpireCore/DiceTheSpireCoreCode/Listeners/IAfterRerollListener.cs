using DiceTheSpireCore.DiceTheSpireCoreCode.Commands;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;

public interface IAfterRerollListener
{
    public Task AfterRerollAsync(CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration);
}