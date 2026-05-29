using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Utility;
using System.Reflection;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;


namespace DiceTheSpireCore.DiceTheSpireCoreCode.Patches;

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
    internal static Task Postfix(Task __result, CardModel __instance, PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        return new Task(async () =>
        {
            if (__instance is ICountdown card)
            {
                if (card.CurrentCount > 0)
                {
                    CardModel[] cardsDiscarded =
                    [
                        ..await CardSelectCmd.FromHandForDiscard(choiceContext, __instance.Owner,
                            new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 0, card.CurrentCount),
                            null, (AbstractModel)card)
                    ];
                    await CardCmd.Discard(choiceContext,
                        cardsDiscarded);
                    card.DecrementCount(cardsDiscarded.Length);
                }

                if (card.CurrentCount == 0)
                {
                    await __result;
                    card.ResetCount();
                }
            }
            else
            {
                await __result;
            }
        });

    }
}