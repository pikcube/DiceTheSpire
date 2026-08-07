using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;

public interface IModifyUnplayableBehaviorListener
{
    public bool ModifyUnplayableBehavior(CardModel card, ref Func<PlayerChoiceContext, CardPlay, Task>? newOnPlay);
}