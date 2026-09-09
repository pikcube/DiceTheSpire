using DiceTheSpire.Shared.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IAfterRerollListener
{
    public Task AfterRerollAsync(PlayerChoiceContext choiceContext, CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration);
}

public interface IAfterFlipListener
{
    public Task AfterFlipAsync(CardModel card, int originalCost, int getAmountToSpend, FlipDuration duration);
}

public interface IAfterNudgeListener
{
    public Task AfterNudgeAsync(CardModel card, int originalCost, int getAmountToSpend, NudgeDuration duration);
}