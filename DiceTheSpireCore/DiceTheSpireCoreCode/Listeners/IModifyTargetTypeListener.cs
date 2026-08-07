using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;

public interface IModifyTargetTypeListener
{
    /// <summary>
    /// Warning: This probably doesn't accomplish what you want it to.
    /// This allows a model to modify the targeting display of cards, but does not modify any of the internal logic.
    /// This is mainly useful for insuring that Unplaybale cards never require declaring a target, which matters for Toolbelt.
    /// If you don't also modify the OnPlay method, you will likely get exceptions from the CardPlay's Target not being set properly.
    /// </summary>
    /// <param name="card">The card to modify.</param>
    /// <param name="result">The new target type.</param>
    /// <returns>True if you modified the target and want to end itteration. False to allow itteration to continue.</returns>
    public bool TryModifyTargetType(CardModel card, ref TargetType result);
}