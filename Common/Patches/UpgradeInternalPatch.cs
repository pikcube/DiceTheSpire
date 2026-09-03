using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Patches;

[HarmonyPatch(typeof(CardModel), "UpgradeInternal")]
public static class UpgradeInternalPatch
{
    public static bool Prefix(CardModel __instance)
    {
        return __instance.IsUpgradable;
    }
}