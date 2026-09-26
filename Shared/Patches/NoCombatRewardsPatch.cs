using DiceTheSpire.Inventor;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(NCombatUi), "OnCombatWon")]
public class NoCombatRewardsPatch
{
    public static bool Prefix(NCombatUi __instance, CombatRoom room)
    {
        if (room.Encounter.ShouldGiveRewards)
        {
            return true;
        }

        TaskHelper.RunSafely(NewOnCombatWon(__instance, room));
        return false;
    }

    private static async Task NewOnCombatWon(NCombatUi nCombatUi, CombatRoom room)
    {
        List<Task> allScrapTasks = [];
        foreach (Player p in RunManager.Instance.PrivatePropertyWrapper<RunManager, RunState>("State").Value?.Players ?? [])
        {
            if (p.Character is not TheInventor ||
                RewardSetOfferPatches.IsEndOfRun(p.RunState, room) ||
                ScrapManager.GadgetIsFromThisFloor(p) ||
                ScrapManager.DebugMode == ScrapManager.ScrapDebugMode.Off)
            {
                continue;
            }
            allScrapTasks.Add(ScrapManager.DoScrapForAsync(p, null));
        }

        await Task.WhenAll(allScrapTasks);
        await nCombatUi.ProceedWithoutRewards();
    }
}