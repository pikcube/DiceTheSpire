namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IRangeCard
{
    /// <summary>
    /// The lowest amount of energy this can randomize to cost. Defaults to 0.
    /// </summary>
    public int MinimumCost { get; }

    /// <summary>
    /// The highest amount of energy this can randomize to cost. Defaults to 3.
    /// </summary>
    public int MaximumCost { get; }
}