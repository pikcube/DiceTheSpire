using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.Shared.Utility;

public static class InventorStaticHoverTips
{
    [CustomEnum] 
    public static StaticHoverTip Scrap = 0;

    [CustomEnum] 
    public static StaticHoverTip TemporaryGadget = 0;

    [CustomEnum]
    public static StaticHoverTip Gadget = 0;
}