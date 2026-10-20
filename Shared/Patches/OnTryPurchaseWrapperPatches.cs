using BaseLib.Abstracts;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(MerchantEntry), nameof(MerchantEntry.OnTryPurchaseWrapper))]
public class OnTryPurchaseWrapperPatches() : CustomSingletonModel(HookType.Run)
{
    public override Task BeforeRoomEntered(AbstractRoom room)
    {
        //Prevents memory leaks by clearing the dictionary's contents on floor change
        Preventers.Clear();
        return Task.CompletedTask;
    }

    public static void Prefix(MerchantEntry __instance, MerchantInventory? inventory, ref bool ignoreCost)
    {
        if (ignoreCost)
        {
            return;
        }

        IRunState? playerRunState = inventory?.Player.RunState;
        if (playerRunState is null)
        {
            return;
        }

        ignoreCost = DiceyHooks.ShouldIgnoreMerchantCost(playerRunState, null, __instance, inventory,
            out IIgnoreMerchantCostListener? merchantCostListener);
        if (merchantCostListener != null)
        {
            Preventers.Add(__instance, merchantCostListener);
        }
    }

    public static async Task<bool> Postfix(Task<bool> __result, MerchantEntry __instance, MerchantInventory? inventory)
    {
        bool result = await __result;

        if (Preventers.TryGetValue(__instance, out IIgnoreMerchantCostListener? value))
        {
            await value.AfterCostIgnored(__instance, inventory, result);
        } 
        
        return result;
    }

    private static Dictionary<MerchantEntry, IIgnoreMerchantCostListener> Preventers { get; set; } = [];
}