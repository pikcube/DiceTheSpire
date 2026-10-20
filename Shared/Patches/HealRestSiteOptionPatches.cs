using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(HealRestSiteOption), nameof(HealRestSiteOption.OnSelect))]
public class HealRestSiteOptionPatches
{
    public static async Task<bool> Postfix(Task<bool> __result, HealRestSiteOption __instance)
    {
        bool result = await __result;

        Player? p = __instance.PrivatePropertyWrapper<RestSiteOption, Player>("Owner").Value;

        if (p is not null)
        {
            await DiceyHooks.BetterAfterRestingAsync(p.RunState, p.Creature.CombatState, p);
        }

        return result;
    }
}