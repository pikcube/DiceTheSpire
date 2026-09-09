using DiceTheSpire.Shared.Commands;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IAfterFlipListener
{
    public Task AfterFlipAsync(CardModel card, int originalCost, int getAmountToSpend, FlipDuration duration);
}