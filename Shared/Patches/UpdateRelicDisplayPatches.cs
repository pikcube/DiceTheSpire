using System.Reflection.Emit;
using DiceTheSpire.Shared.Relics;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(NInspectRelicScreen), "UpdateRelicDisplay")]
public static class UpdateRelicDisplayPatches
{
    [UsedImplicitly]
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher matcher = new(instructions);
        matcher.MatchStartForward(CodeMatch.WithOpcodes([OpCodes.Ret]));
        matcher.MatchStartBackwards(CodeMatch.WithOpcodes([OpCodes.Newobj]))
            .ThrowIfInvalid("Could not find where LocString is created")
            .RemoveInstruction()
            .InsertAndAdvance()
            .InsertAndAdvance(CodeInstruction.LoadLocal(0))
            .InsertAndAdvance(CodeInstruction.Call(() => CreateLocString(null!, null!, null!)));

        return matcher.Instructions();
    }
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

    [UsedImplicitly]
    private static LocString CreateLocString(string table, string entry, RelicModel relic)
    {
        return relic is Gadget ? new LocString("gadgets", "DICETHESPIRE-GADGET.type") : new LocString(table, entry);
    }
}