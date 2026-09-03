using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Listeners;

public interface IModifyUnplayableBehaviorListener
{
    public bool ModifyUnplayableBehavior(CardModel card);
    /// <summary>
    /// This allows a model to modify the targeting display of cards, but does not modify any of the internal logic.
    /// This is mainly useful for insuring that Unplaybale cards never require declaring a target, which matters for Toolbelt.
    /// If you don't also modify the OnPlay method, you will likely get exceptions from the CardPlay's Target not being set properly.
    /// </summary>
    /// <param name="card">The card to modify.</param>
    /// <param name="result">The new target type.</param>
    /// <returns>True if you modified the target and want to end itteration. False to allow itteration to continue.</returns>
    public bool TryModifyTargetType(CardModel card, ref TargetType result);

    public bool TryModifyOnPlay(CardModel card, ref Func<PlayerChoiceContext, CardPlay, Task> task);
}