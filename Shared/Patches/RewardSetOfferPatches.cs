using DiceTheSpire.Inventor;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(RewardsSet), nameof(RewardsSet.Offer))]
public static class RewardSetOfferPatches
{
    public static bool Prefix(RewardsSet __instance, ref Task __result)
    {
        if (__instance.Player.Character is not TheInventor ||
            __instance.Player.RunState.CurrentRoom is not CombatRoom cr ||
            IsEndOfRun(__instance.Player.RunState, cr) ||
            ScrapManager.GadgetIsFromThisFloor(__instance.Player)||
            ScrapManager.DebugMode == ScrapManager.ScrapDebugMode.Off)
        {
            return true;
        }

        __result = ScrapManager.DoScrapForThenOfferAsync(__instance);
        return false;
    }

    private static bool IsEndOfRun(IRunState runState, CombatRoom room)
    {
        return room.RoomType == RoomType.Boss && runState.CurrentActIndex >= runState.Acts.Count - 1;
    }

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(RewardsSet), nameof(RewardsSet.Offer))]
    public static Task OfferRewardAfterScrapAsync(object instance)
    {
        _ = instance;
        return Task.CompletedTask;
    }
}