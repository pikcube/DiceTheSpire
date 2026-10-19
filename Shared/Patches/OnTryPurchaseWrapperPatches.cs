using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Merchant;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(MerchantEntry), nameof(MerchantEntry.OnTryPurchaseWrapper))]
public static class OnTryPurchaseWrapperPatches
{
    public static void Prefix(MerchantEntry __instance, MerchantInventory? inventory, ref bool ignoreCost)
    {
        if (ignoreCost)
        {
            return;
        }

        ignoreCost = DiceyHooks.ShouldIgnoreMerchantCost(__instance, inventory,
            out DiceyHooks.IIgnoreMerchantCostListener? merchantCostListener);
        if (merchantCostListener != null)
        {
            Preventers.Add(__instance, merchantCostListener);
        }
    }

    public static async Task<bool> Postfix(Task<bool> __result, MerchantEntry __instance, MerchantInventory? inventory, bool ignoreCost)
    {
        bool result = await __result;

        if (Preventers.ContainsKey(__instance))
        {
            await Preventers[__instance].AfterCostIgnored(__instance, inventory, result);
        } 
        
        return result;
    }

    private static Dictionary<MerchantEntry, DiceyHooks.IIgnoreMerchantCostListener> Preventers { get; set; } = [];
}