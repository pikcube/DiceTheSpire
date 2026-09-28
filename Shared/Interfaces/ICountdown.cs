using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Interfaces;

public interface ICountdown
{
    public int BaseCount { get; set; }
    public List<LocalCostModifier> CountdownCostModifiers { get; set; }

    public CardModel AsCardModel();
}