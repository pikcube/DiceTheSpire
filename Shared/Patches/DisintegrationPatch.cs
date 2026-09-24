using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Cards;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(Disintegration), nameof(Disintegration.OnChosen))]
public class DisintegrationPatch() : CustomSingletonModel(HookType.Combat)
{
    public static bool IsKnowledgeDemonDoingStuff => DisintegrationsInProgress.Count > 0;

    private static List<Disintegration> DisintegrationsInProgress { get; } = [];

    public override Task BeforeCombatStart()
    {
        DisintegrationsInProgress.Clear();
        return Task.CompletedTask;
    }

    public static void Prefix(Disintegration __instance)
    {
        DisintegrationsInProgress.Add(__instance);
    }

    public static async Task Postfix(Task __result, Disintegration __instance)
    {
        await __result;
        DisintegrationsInProgress.Remove(__instance);
    }
}