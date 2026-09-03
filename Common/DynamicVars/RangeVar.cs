using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.DynamicVars;

public class MinRangeVar : DynamicVar
{
    internal MinRangeVar(int min) : base("RangeMin", min)
    {
    }
}

public class MaxRangeVar : DynamicVar
{
    internal MaxRangeVar(int max) : base("RangeMax", max)
    {
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.HoverTips), MethodType.Getter)]
public static class RangeVars
{

    public static IEnumerable<IHoverTip> Postfix(IEnumerable<IHoverTip> __result, CardModel __instance)
    {
        IEnumerable<IHoverTip> tips = [.. __result, .. RangeHoverTip(__instance)];
        return tips.Distinct();
    }


    public static IEnumerable<DynamicVar> Make(int min, int max)
    {
        return [new MinRangeVar(min), new MaxRangeVar(max)];
    }

    public static bool TryGet(CardModel card, out int min, out int max)
    {
        MinRangeVar? minVar = card.DynamicVars.Values.OfType<MinRangeVar>().SingleOrDefault();
        MaxRangeVar? maxVar = card.DynamicVars.Values.OfType<MaxRangeVar>().SingleOrDefault();
        if (minVar is null || maxVar is null)
        {
            min = -1;
            max = -1;
            return false;
        }

        min = minVar.IntValue;
        max = maxVar.IntValue;
        return true;
    }
    private static LocString RangeTitle => new("static_hover_tips", $"{DiceTheSpire.MainFile.ModPrefix}-RANGE.title");

    private static LocString OneRangeText => new("static_hover_tips", $"{DiceTheSpire.MainFile.ModPrefix}-RANGE.oneDescription");

    private static LocString TwoRangeText => new("static_hover_tips", $"{DiceTheSpire.MainFile.ModPrefix}-RANGE.twoDescription");

    public static IEnumerable<IHoverTip> RangeHoverTip(CardModel card)
    {
        if (!TryGet(card, out int min, out int max))
        {
            yield break;
        }

        if (min == 0 && max == 3)
        {
            yield break;
        }

        if (min == max)
        {
            LocString oneRange = OneRangeText;
            oneRange.Add("cost", max);
            LocString title = RangeTitle;
            title.Add("values", max);
            yield return new HoverTip(title, oneRange);
        }
        else
        {
            LocString twoRange = TwoRangeText;
            twoRange.Add("min", min);
            twoRange.Add("max", max);
            LocString title = RangeTitle;
            title.Add("values", $"{min}, {max}");
            yield return new HoverTip(title, twoRange);
        }

    }
}