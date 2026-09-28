using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.SetToFreeThisCombat))]
public static class FreeToPlayCardThisCombatPatches
{
    public static void Postfix(CardModel __instance)
    {
        if (__instance is ICountdown countdown)
        {
            countdown.SetCountThisCombat(0);
        }
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.SetToFreeThisTurn))]
public static class FreeToPlayCardThisTurnPatches
{
    public static void Postfix(CardModel __instance)
    {
        if (__instance is ICountdown countdown)
        {
            countdown.SetCountThisTurnOrUntilPlayed(0);
        }
    }
}

[HarmonyPatch(typeof(CardEnergyCost), nameof(CardEnergyCost.EndOfTurnCleanup))]
public static class EndOfTurnCleanupPatches
{
    public static bool Postfix(bool __result, CardEnergyCost __instance)
    {
        if (__instance.PrivateFieldWrapper<CardEnergyCost, CardModel>("_card").Value is ICountdown countdown)
        {
            countdown.CountdownCostModifiers.RemoveAll(mod => mod.Expiration.HasFlag(LocalCostModifierExpiration.EndOfTurn));
        }

        return __result;
    }
}

[HarmonyPatch(typeof(CardEnergyCost), nameof(CardEnergyCost.AfterCardPlayedCleanup))]
public static class AfterPlayCleanupPatches
{
    public static bool Postfix(bool __result, CardEnergyCost __instance)
    {
        if (__instance.PrivateFieldWrapper<CardEnergyCost, CardModel>("_card").Value is ICountdown countdown)
        {
            countdown.CountdownCostModifiers.RemoveAll(mod => mod.Expiration.HasFlag(LocalCostModifierExpiration.WhenPlayed));
        }

        return __result;
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.CostsEnergyOrStars))]
public static class CostsEnergyOrStarsPatch
{
    public static bool Postfix(bool __result, CardModel __instance, bool includeGlobalModifiers)
    {

        if (__instance is ICountdown countdown)
        {
            return __result || countdown.GetCount(includeGlobalModifiers) > 0;
        }

        return __result;
    }
}