using BaseLib.Patches.Content;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.Shared.Utility;

public static class BetterStaticHoverTips
{
    [CustomEnum, UsedImplicitly] 
    public static StaticHoverTip Bump = 0;

    [CustomEnum, UsedImplicitly]
    public static StaticHoverTip Rummage = 0;

    [CustomEnum, UsedImplicitly]
    public static StaticHoverTip Reroll = 0;

    [CustomEnum, UsedImplicitly]
    public static StaticHoverTip Nudge = 0;

    [CustomEnum, UsedImplicitly] 
    public static StaticHoverTip Inspect = 0;

    [CustomEnum, UsedImplicitly]
    public static StaticHoverTip Held = 0;

    [CustomEnum, UsedImplicitly]
    public static StaticHoverTip Flip = 0;

    [CustomEnum, UsedImplicitly]
    public static StaticHoverTip Scrap = 0;

    [CustomEnum, UsedImplicitly]
    public static StaticHoverTip TemporaryGadget = 0;

    [CustomEnum, UsedImplicitly]
    public static StaticHoverTip Gadget = 0;
}