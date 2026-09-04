using System.Reflection;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.CanPlay), [typeof(UnplayableReason), typeof(AbstractModel)], [ArgumentType.Out, ArgumentType.Out])]
public static class ModifyUnplayableBehaviorPatches
{ 
    public static bool Postfix(bool __result, CardModel __instance, ref UnplayableReason reason, ref AbstractModel? preventer)
    {
        if (__result || __instance.RunState is null || (reason ^ UnplayableReason.HasUnplayableKeyword) != 0)
        {
            return __result;
        }

        bool shouldModify = DiceyHooks.OnModifyUnplayableBehavior(__instance.RunState, __instance);

        if (!shouldModify)
        {
            return __result;
        }

        reason = UnplayableReason.None;
        preventer = null;
        return shouldModify;
    }
}

[HarmonyPatch]
public static class ModifyCardOnPlay
{
    [UsedImplicitly]
    static IEnumerable<MethodBase> TargetMethods()
    {
        MethodInfo? onPlayMethod = typeof(CardModel).GetMethod("OnPlay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (onPlayMethod is not null)
        {
            yield return onPlayMethod;
        }

        foreach (Type? type in AccessTools.AllTypes()
                     .Where(t => t.IsSubclassOf(typeof(CardModel)) && !t.IsAbstract))
        {
            MethodInfo? method = type.GetMethod("OnPlay",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method != null && method.DeclaringType != typeof(CardModel))
            {
                yield return method;
            }
        }
    }

    public static bool Prefix(ref Task __result, CardModel __instance, PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (__instance.RunState is null || !__instance.Keywords.Contains(CardKeyword.Unplayable))
        {
            return true;
        }

        if (!DiceyHooks.TryModifyUnplayableOnPlay(__instance.RunState, __instance, out Func<PlayerChoiceContext, CardPlay, Task> newOnPlay))
        {
            return true;
        }

        __result = newOnPlay(choiceContext, cardPlay);
        return false;

    }
}