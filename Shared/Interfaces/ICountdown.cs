using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Interfaces;

public interface ICountdown
{
    public int BaseCount { get; set; }
    public bool HasCountdownCostX { get; }
    public int? CountSpent { get; set; }
    public int? CountValue { get; set; }
    public List<LocalCostModifier> CountdownCostModifiers { get; set; }

    public CardModel AsCardModel();
}