using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Cards;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(WasteAway), nameof(WasteAway.OnChosen))]
public class WasteAwayPatch() : CustomSingletonModel(HookType.Combat)
{
    public static bool IsKnowledgeDemonDoingStuff => WasteAwaysInProgress.Count > 0;

    private static List<WasteAway> WasteAwaysInProgress { get; } = [];

    public override Task BeforeCombatStart()
    {
        WasteAwaysInProgress.Clear();
        return Task.CompletedTask;
    }

    public static void Prefix(WasteAway __instance)
    {
        WasteAwaysInProgress.Add(__instance);
    }

    public static async Task Postfix(Task __result, WasteAway __instance)
    {
        await __result;
        WasteAwaysInProgress.Remove(__instance);
    }
}