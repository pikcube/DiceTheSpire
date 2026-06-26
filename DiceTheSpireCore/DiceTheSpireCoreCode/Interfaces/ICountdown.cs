using MegaCrit.Sts2.Core.Entities.Players;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface ICountdown
{
    public int MaxCount { get; set; }
    public int CurrentCount { get; set; }
    Player Owner { get;}
}