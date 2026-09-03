namespace DiceTheSpire.Common.Interfaces;

public interface IFuryModifier
{
    public bool ShouldIgnoreFury { get; }
    public bool ShouldMaintainFury { get; }
}