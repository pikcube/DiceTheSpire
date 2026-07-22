using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Models;
using System.Reflection;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

[HarmonyPatch]
public class ModifyCardTargetTypePatch
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
        if (__instance.RunState is not null)
        {
            DiceyHooks.OnModifyTargetType(__instance.RunState, __instance, ref __result);
        }
        return __result;
    }
}