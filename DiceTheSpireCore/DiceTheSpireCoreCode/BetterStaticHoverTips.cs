using BaseLib.Patches.Content;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace DiceTheSpireCore.DiceTheSpireCoreCode;

public static class BetterStaticHoverTips
{
    [CustomEnum] 
    public static StaticHoverTip Bump = 0;

    [CustomEnum]
    public static StaticHoverTip Rummage = 0;

    [CustomEnum]
    public static StaticHoverTip Reroll = 0;

    [CustomEnum]
    public static StaticHoverTip Nudge = 0;

    [CustomEnum] 
    public static StaticHoverTip Inspect = 0;

    [CustomEnum]
    public static StaticHoverTip Held = 0;

    private static LocString RangeTitle
    {
        get
        {
            field ??= new LocString("static_hover_tips", "DICETHESPIRECORE-RANGE.title");
            return field;
        }
    }

    private static LocString OneRangeText => new("static_hover_tips", "DICETHESPIRECORE-RANGE.oneDescription");

    private static LocString TwoRangeText => new("static_hover_tips", "DICETHESPIRECORE-RANGE.twoDescription");

    public static HoverTip RangeHoverTip(IRangeCard card)
    {
        if (card.MinimumCost == card.MaximumCost)
        {
            LocString oneRange = OneRangeText;
            oneRange.Add("cost", card.MaximumCost);
            return new HoverTip(RangeTitle, oneRange);
        }

        LocString twoRange = TwoRangeText;
        twoRange.Add("min", card.MinimumCost);
        twoRange.Add("max", card.MaximumCost);
        return new HoverTip(RangeTitle, twoRange);
    }
}