using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;

public interface IModifyTargetTypeListener
{
    public bool TryModifyTargetType(CardModel card, ref TargetType result);
}