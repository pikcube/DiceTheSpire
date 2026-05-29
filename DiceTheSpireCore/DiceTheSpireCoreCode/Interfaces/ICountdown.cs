using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface ICountdown
{
    public int MaxCount { get; set; }
    public int CurrentCount { get; set; }
    public Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay);
}