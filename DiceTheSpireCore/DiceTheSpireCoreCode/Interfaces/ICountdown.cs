using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface ICountdown
{
    public int MaxCount { get; set; }
    public int CurrentCount { get; set; }
    Player Owner { get;}
    public Task OnCountdownZero(PlayerChoiceContext choiceContext, CardPlay cardPlay);
}