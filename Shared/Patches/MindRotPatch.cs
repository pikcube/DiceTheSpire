using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Cards;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(MindRot), nameof(MindRot.OnChosen))]
public class MindRotPatch() : CustomSingletonModel(HookType.Combat)
{
    public static bool IsKnowledgeDemonDoingStuff => MindRotsInProgress.Count > 0;

    private static List<MindRot> MindRotsInProgress { get; } = [];

    public override Task BeforeCombatStart()
    {
        MindRotsInProgress.Clear();
        return Task.CompletedTask;
    }

    public static void Prefix(MindRot __instance)
    {
        MindRotsInProgress.Add(__instance);
    }

    public static async Task Postfix(Task __result, MindRot __instance)
    {
        await __result;
        MindRotsInProgress.Remove(__instance);
    }
}