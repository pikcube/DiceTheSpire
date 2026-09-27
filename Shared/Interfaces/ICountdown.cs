using MegaCrit.Sts2.Core.Entities.Players;

namespace DiceTheSpire.Shared.Interfaces;

public interface ICountdown
{
    public int MaxCount { get; set; }
    public Player Owner { get; }
}