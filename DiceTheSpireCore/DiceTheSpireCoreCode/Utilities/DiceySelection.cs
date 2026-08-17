using MegaCrit.Sts2.Core.Localization;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;

public static class DiceySelection
{
    public static LocString ToBlink => new("card_selection", "DICETHESPIRECORE-TO_BLINK");
    public static LocString ToBump => new("card_selection", "DICETHESPIRECORE-TO_BUMP");
    public static LocString ToNudge => new("card_selection", "DICETHESPIRECORE-TO_NUDGE");
    public static LocString ToDupe => new("card_selection", "DICETHESPIRECORE-TO_DUPE");
    public static LocString ToPull => new("card_selection", "DICETHESPIRECORE-TO_PULL");
    public static LocString ToModifyCost => new("card_selection", "DICETHESPIRECORE-TO_MODIFY_COST");
    public static LocString ToCountdown => new("card_selection", "DICETHESPIRECORE-TO_COUNTDOWN");
    public static LocString ToRandomize => new("card_selection", "DICETHESPIRECORE-TO_RANDOMIZE");
}