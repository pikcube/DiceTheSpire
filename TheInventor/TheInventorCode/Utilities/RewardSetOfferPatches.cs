using HarmonyLib;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace TheInventor.TheInventorCode.Utilities;

[HarmonyPatch(typeof(RewardsSet), nameof(RewardsSet.Offer))]
public class RewardSetOfferPatches
{
    public static bool Prefix(RewardsSet __instance, ref Task __result)
    {
        if (__instance.Player.Character is not Character.TheInventor || __instance.Player.RunState.CurrentRoom is not CombatRoom || ScrapManager.ScrapComplete(__instance.Player))
        {
            return true;
        }

        __result = ScrapManager.DoScrapAsyncFor(__instance);
        return false;
    }
}