using MegaCrit.Sts2.Core.Entities.Players;

namespace DiceTheSpire.Shared.Interfaces;

public interface ICountdown
{
    public int BaseCount { get; set; }
    public Player Owner { get; }
}