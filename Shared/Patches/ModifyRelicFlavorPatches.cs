using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(RelicModel), nameof(RelicModel.Flavor), MethodType.Getter)]
public static class ModifyRelicFlavorPatches
{
    public static LocString Postfix(LocString __result, RelicModel __instance)
    {
        return DiceyHooks.ModifyRelicFlavor(__result, __instance);
    }
}