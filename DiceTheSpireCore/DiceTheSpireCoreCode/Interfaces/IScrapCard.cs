using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IScrapCard
{
    public bool IsAlwaysOfferedAsScrap { get; }
}

public interface IScrapCard<out T> : IScrapCard where T : CardModel
{
    public T Card { get; }
}