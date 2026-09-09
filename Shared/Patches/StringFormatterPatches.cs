using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using SmartFormat;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(LocManager), "LoadLocFormatters")]
public static class StringFormatterPatches
{
    public static void Postfix()
    {
        Smart.Default.AddExtensions(new DiceIconFormatter());
    }
}