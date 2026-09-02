using DiceTheSpire.DiceTheSpireCode.Inventor;
using DiceTheSpire.DiceTheSpireCode.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

[HarmonyPatch(typeof(RewardsSet), nameof(RewardsSet.Offer))]
public class RewardSetOfferPatches
{
    public static bool Prefix(RewardsSet __instance, ref Task __result)
    {
        if (__instance.Player.Character is not TheInventor || 
            __instance.Player.RunState.CurrentRoom is not CombatRoom cr ||
            IsEndOfRun(__instance.Player.RunState, cr) ||
            ScrapManager.ScrapComplete(__instance.Player))
        {
            return true;
        }

        __result = ScrapManager.DoScrapAsyncFor(__instance);
        return false;
    }

    private static bool IsEndOfRun(IRunState runState, CombatRoom room)
    {
        return room.RoomType == RoomType.Boss && runState.CurrentActIndex >= runState.Acts.Count - 1;
    }
}