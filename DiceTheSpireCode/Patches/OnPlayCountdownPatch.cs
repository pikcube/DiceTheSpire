using System.Reflection;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

[HarmonyPatch]
public class OnPlayCountdownPatch
{
    [UsedImplicitly]
    static IEnumerable<MethodBase> TargetMethods()
    {
        MethodInfo? onPlayMethod = typeof(CardModel).GetMethod("OnPlay");
        if (onPlayMethod is not null)
        {
            yield return onPlayMethod;
        }

        foreach (Type? type in AccessTools.AllTypes()
                     .Where(t => t.IsSubclassOf(typeof(CardModel)) && !t.IsAbstract))
        {
            MethodInfo? method = type.GetMethod("OnPlay",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method != null && method.DeclaringType != typeof(CardModel))
            {
                yield return method;
            }
        }
    }

    [UsedImplicitly]
    internal static bool Prefix(CardModel __instance)
    {
        return __instance is not ICountdown card || card.CurrentCount == 0;
    }


    [UsedImplicitly]
    internal static Task Postfix(Task __result, CardModel __instance, bool __runOriginal, PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        return __instance is not ICountdown card ? __result : PatchFunction();

        async Task PatchFunction()
        {
            if (card.CurrentCount > 0)
            {
                CardModel[] cardsDiscarded =
                [
                    ..await CardSelectCmd.FromHandForDiscard(choiceContext, __instance.Owner,
                        new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 0, card.CurrentCount),
                        null, (AbstractModel)card)
                ];
                await CardCmd.Discard(choiceContext, cardsDiscarded);
                card.DecrementCount(cardsDiscarded.Length);

                if (card.CurrentCount == 0)
                {
                    await card.OnPlay(choiceContext, cardPlay);
                }
            }
            else
            {
                card.ResetCount();
            }
        }
    }
}