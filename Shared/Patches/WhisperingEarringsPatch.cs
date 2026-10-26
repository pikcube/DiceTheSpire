using DiceTheSpire.Shared.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using System.Reflection;
using JetBrains.Annotations;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(WhisperingEarring), nameof(WhisperingEarring.AfterAutoPrePlayPhaseEnteredLate), MethodType.Async)]
public static class WhisperingEarringsPatch
{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        CodeMatcher matcher = new(instructions);
        //Stack currently contains a Task<(int, int)>
        matcher.MatchStartForward(CodeMatch.Calls(() => default(Task<(int, int)>)!.GetAwaiter()))
            .ThrowIfInvalid("Could not find where SpendResources is executed")
        //Load this
            .InsertAndAdvance(CodeInstruction.LoadArgument(0))
        //Consume this, load 'card'
            .InsertAndAdvance(CodeInstruction.LoadField(original.DeclaringType, "<card>5__6"))
        //Load this
            .InsertAndAdvance(CodeInstruction.LoadArgument(0))
        //Consume this, load 'choiceContext'
            .InsertAndAdvance(CodeInstruction.LoadField(original.DeclaringType, "choiceContext"))
        //Consume SpendResources Task<(int, int), card, and choiceContext, load Task<(int, int)
            .InsertAndAdvance(CodeInstruction.Call(() => SpendDiceyResourcesAsync(null!, null!, null!)));

        return matcher.InstructionEnumeration();
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

    [UsedImplicitly]
    private static async Task<(int, int)> SpendDiceyResourcesAsync(Task<(int, int)> result, CardModel card, PlayerChoiceContext context)
    {
        (int, int) spent = await result;

        if (card is DiceTheSpireCard diceyCard)
        {
            await diceyCard.DiceySpendResourcesAsync(context);
        }

        return spent;
    }
}