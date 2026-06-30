using HarmonyLib;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Patches;

[HarmonyPatch(typeof(RewardsSet), nameof(RewardsSet.Offer))]
public class RewardSetOfferPatches
{
    public static bool Prefix(RewardsSet __instance, ref Task __result)
    {
        if (__instance.Player.Character is not Character.TheInventor || 
            __instance.Player.RunState.CurrentRoom is not CombatRoom cr ||
            IsEndOfRun(__instance.Player.RunState, cr) ||
            ScrapManager.ScrapComplete(__instance.Player))
        {
            return true;
        }

        __result = ScrapManager.DoScrapAsyncFor(__instance);
        return false;
    }

    private static bool IsEndOfRun(IRunState runState, CombatRoom cr)
    {
        if (cr.Act.Index + 1 != runState.Acts.Count)
        {
            return false;
        }

        return cr.Encounter.Id == cr.Act.BossEncounter.Id || cr.Encounter.Id == cr.Act.SecondBossEncounter?.Id;
    }
}