using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;

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

    [CustomEnum]
    public static StaticHoverTip Flip = 0;
}