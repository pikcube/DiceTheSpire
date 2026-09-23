using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Cards;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(Sloth), nameof(Sloth.OnChosen))]
public class SlothPatch() : CustomSingletonModel(HookType.Combat)
{
    public static bool IsKnowledgeDemonDoingStuff => SlothsInProgress.Count > 0;

    private static List<Sloth> SlothsInProgress { get; } = [];

    public override Task BeforeCombatStart()
    {
        SlothsInProgress.Clear();
        return Task.CompletedTask;
    }

    public static void Prefix(Sloth __instance)
    {
        SlothsInProgress.Add(__instance);
    }

    public static async Task Postfix(Task __result, Sloth __instance)
    {
        await __result;
        SlothsInProgress.Remove(__instance);
    }
}