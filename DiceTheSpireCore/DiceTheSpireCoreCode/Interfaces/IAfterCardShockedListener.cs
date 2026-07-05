using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IAfterCardShockedListener
{
    public Task AfterCardShockedAsync(CardModel card);
}