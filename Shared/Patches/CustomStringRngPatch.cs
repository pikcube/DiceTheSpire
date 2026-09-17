using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(RunRngSet), MethodType.Constructor, typeof(string))]
public static class CustomStringRngPatch
{
    public static void Postfix(RunRngSet __instance, string seed, Dictionary<RunRngType, Rng> ____rngs)
    {
        foreach (RunRngType type in DiceyRng.All)
        {
            ____rngs.Add(type, new Rng(__instance.Seed, StringHelper.SnakeCase(type.ToString())));
        }
    }
}

[HarmonyPatch(typeof(RunRngSet), MethodType.Constructor, typeof(Rng))]
public static class CustomRngRngPatch
{
    public static void Postfix(RunRngSet __instance, Rng rng, Dictionary<RunRngType, Rng> ____rngs)
    {
        foreach (RunRngType type in DiceyRng.All)
        {
            ____rngs.Add(type, rng);
        }
    }
}