using System.Reflection;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Utility;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch]
public static class ModifyCardTargetTypePatch
{
    [UsedImplicitly]
    static IEnumerable<MethodBase> TargetMethods() =>
        PatchingUtilities.TargetGetterOverrides(typeof(CardModel), nameof(CardModel.TargetType));

    public static TargetType Postfix(TargetType __result, CardModel __instance)
    {
        if (!__instance.Keywords.Contains(CardKeyword.Unplayable))
        {
            return __result;
        }
        if (__instance.RunState is not null)
        {
            DiceyHooks.OnModifyTargetType(__instance.RunState, __instance.CombatState, __instance, ref __result);
        }
        return __result;
    }
}