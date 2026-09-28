using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface ICountdownCostListener
{
    public int ModifyCost(ICountdown countdown, CardModel card, int cost);
}