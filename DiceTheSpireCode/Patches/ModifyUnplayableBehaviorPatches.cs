using System.Reflection;
using System.Runtime.CompilerServices;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.CanPlay), [typeof(UnplayableReason), typeof(AbstractModel)], [ArgumentType.Out, ArgumentType.Out])]
public static class ModifyUnplayableBehaviorPatches
{
    public static readonly ConditionalWeakTable<CardModel, Func<PlayerChoiceContext, CardPlay, Task>> OnPlayReplacements = [];

    public static bool Postfix(bool __result, CardModel __instance, ref UnplayableReason reason, ref AbstractModel? preventer)
    {
        if (__result || __instance.RunState is null || (reason ^ UnplayableReason.HasUnplayableKeyword) != 0)
        {
            OnPlayReplacements.Remove(__instance);
            return __result;
        }

        bool shouldModify = DiceyHooks.OnModifyUnplayableBehavior(__instance.RunState, __instance,
            out Func<PlayerChoiceContext, CardPlay, Task>? play);

        if (!shouldModify)
        {
            OnPlayReplacements.Remove(__instance);
            return __result;
        }

        play ??= (_, _) => Task.CompletedTask;
        reason = UnplayableReason.None;
        preventer = null;
        OnPlayReplacements.AddOrUpdate(__instance, play);
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
        if (!ModifyUnplayableBehaviorPatches.OnPlayReplacements.TryGetValue(__instance,
                out Func<PlayerChoiceContext, CardPlay, Task>? func))
        {
            return true;
        }

        __result = func(choiceContext, cardPlay);
        return false;

    }
}