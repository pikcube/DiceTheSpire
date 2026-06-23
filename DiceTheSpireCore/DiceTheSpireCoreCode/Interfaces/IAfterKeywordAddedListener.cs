using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IAfterKeywordAddedListener
{
    public Task AfterKeywordAddedAsync(CardModel card, CardKeyword keyword);
}