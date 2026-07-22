using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IModifyTargetType
{
    public bool TryModifyTargetType(CardModel card, ref TargetType result);
}