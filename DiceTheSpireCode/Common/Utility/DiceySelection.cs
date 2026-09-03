using MegaCrit.Sts2.Core.Localization;

namespace DiceTheSpire.DiceTheSpireCode.Common.Utility;

public static class DiceySelection
{
    public static LocString ToBlink => new("card_selection", $"{MainFile.ModPrefix}-TO_BLINK");
    public static LocString ToBump => new("card_selection", $"{MainFile.ModPrefix}-TO_BUMP");
    public static LocString ToNudge => new("card_selection", $"{MainFile.ModPrefix}-TO_NUDGE");
    public static LocString ToDupe => new("card_selection", $"{MainFile.ModPrefix}-TO_DUPE");
    public static LocString ToPull => new("card_selection", $"{MainFile.ModPrefix}-TO_PULL");
    public static LocString ToModifyCost => new("card_selection", $"{MainFile.ModPrefix}-TO_MODIFY_COST");
    public static LocString ToCountdown => new("card_selection", $"{MainFile.ModPrefix}-TO_COUNTDOWN");
    public static LocString ToRandomize => new("card_selection", $"{MainFile.ModPrefix}-TO_RANDOMIZE");
    public static LocString ToFlip => new("card_selection", $"{MainFile.ModPrefix}-TO_FLIP");
}