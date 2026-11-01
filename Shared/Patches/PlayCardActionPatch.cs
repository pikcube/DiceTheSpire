using System.Reflection.Emit;
using DiceTheSpire.Shared.Cards;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Patches;


[HarmonyPatch(typeof(PlayCardAction), "ExecuteAction", MethodType.Async)]
[HarmonyPriority(int.MaxValue)]
public static class PlayCardActionPatch
{
    [UsedImplicitly]
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher matcher = new(instructions);

        //Match SpendResources's awaiter and prepend with call to CreateChoiceAndSpendResourcesAsync
        matcher.MatchStartForward(CodeMatch.Calls(() => default(Task<(int, int)>)!.GetAwaiter()))
            .ThrowIfInvalid("Could not find get awaiter")
            //Load PlayCardAction to the evaluation stack
            .InsertAndAdvance(CodeInstruction.LoadLocal(1))
            //Consume Task<(int, int)> and PlayCardAction, load Task<(int, int)> to the stack 
            .InsertAndAdvance(CodeInstruction.Call(() => CreateChoiceAndSpendResourcesAsync(null!, null!)));

        //Match and remove setting of PCC
        matcher.MatchStartForward(CodeMatch.Calls(typeof(PlayCardAction).DeclaredProperty("PlayerChoiceContext").SetMethod))
            .ThrowIfInvalid("Could not find where PCC is set")
            .RemoveInstruction();

        //Match and remove creation of PCC which is consumed by set PCC
        matcher.MatchStartBackwards(CodeMatch.WithOpcodes([OpCodes.Newobj]))
            .ThrowIfInvalid("Could not find where PCC is created")
            .RemoveInstruction();

        //Match and remove load of this which is consumed by set PCC
        matcher.MatchStartBackwards(CodeMatch.WithOpcodes([OpCodes.Ldloc_1]))
            .ThrowIfInvalid("Could not find load of 'this' to evaluation stack.")
            .RemoveInstruction();

        //Match and remove load of this which is consumed by set PCC. This isn't an error, there's two ldloc.1 instructions to remove
        matcher.MatchStartBackwards(CodeMatch.WithOpcodes([OpCodes.Ldloc_1]))
            .ThrowIfInvalid("Could not find load of 'this' to evaluation stack")
            .RemoveInstruction();

        return matcher.Instructions();
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning restore CA1859 // Use concrete types when possible for improved performance


    public static async Task<(int, int)> CreateChoiceAndSpendResourcesAsync(Task<(int, int)> originalTask, PlayCardAction action)
    {
        (int, int) result = await originalTask;
        CardModel card = action.PrivateFieldWrapper<PlayCardAction, CardModel>("_card").Value ?? throw new NullReferenceException();

        GameActionPlayerChoiceContext pcc = new(action);
        action.PrivatePropertyWrapper<PlayCardAction, PlayerChoiceContext>("PlayerChoiceContext").Value = pcc;

        if (card is DiceTheSpireCard diceyCard)
        {
            await diceyCard.DiceySpendResourcesAsync(pcc);
        }

        return result;
    }
}