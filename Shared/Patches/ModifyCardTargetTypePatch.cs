using System.Reflection;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch]
public static class ModifyCardTargetTypePatch
{
    [UsedImplicitly]
    static IEnumerable<MethodBase> TargetMethods()
    {
        MethodInfo? onPlayMethod = typeof(CardModel).GetProperty(nameof(CardModel.TargetType))?.GetMethod;
        if (onPlayMethod is not null)
        {
            yield return onPlayMethod;
        }

        foreach (Type? type in AccessTools.AllTypes()
                     .Where(t => t.IsSubclassOf(typeof(CardModel)) && !t.IsAbstract))
        {
            MethodInfo? method = type.GetProperty(nameof(CardModel.MutableClone),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetMethod;

            if (method != null && method.DeclaringType != typeof(CardModel))
            {
                yield return method;
            }
        }
    }

    public static TargetType Postfix(TargetType __result, CardModel __instance)
    {
        if (!__instance.Keywords.Contains(CardKeyword.Unplayable))
        {
            return __result;
        }
        if (__instance.RunState is not null)
        {
            DiceyHooks.OnModifyTargetType(__instance.RunState, __instance, ref __result);
        }
        return __result;
    }
}