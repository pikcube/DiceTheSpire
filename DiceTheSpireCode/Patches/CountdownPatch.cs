using DiceTheSpire.DiceTheSpireCode.Utility;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
public static class CountdownPatch
{
    //All branches eventually execute the OnPlayWrapper, the question is whether there is any sandwich logic happening before and after or not.
    public static bool Prefix(ref Task __result, CardModel __instance, PlayerChoiceContext choiceContext, Creature? target,
        bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals = false)
    {
        //Don't touch it if it isn't a countdown.
        if (__instance is not ICountdown countdown)
        {
            return true;
        }

        //If the card is Autoplayed, we need to run logic to execute as if the card immediately reached a countdown of 0.
        if (isAutoPlay)
        {
            __result = TriggerCountdownAutoplay(__instance, countdown, choiceContext, target, isAutoPlay, resources, skipCardPileVisuals);
            return false;
        }

        //If the current countdown is 0, or we have marked that the play needs cancelled, we allow normal execution through.
        if (countdown.CurrentCount == 0  || CountdownCanceller.IsCancelled(countdown))
        {
            return true;
        }

        //Otherwise, we need to prompt the user to execute before allowing the base case to execute.
        __result = ResolveCountdownPrefix(__instance, countdown, choiceContext, target, isAutoPlay, resources, skipCardPileVisuals);
        return false;
    }

    //This function just decrements the countdown to 0, plays the card, then resets the countdown to its max value.
    private static async Task TriggerCountdownAutoplay(CardModel cardModel, ICountdown countdown, PlayerChoiceContext choiceContext, Creature? target, bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals)
    {
        while (countdown.CurrentCount > 0)
        {
            await countdown.DecrementCountAsync(countdown.CurrentCount);
        }
        await cardModel.OnPlayWrapper(choiceContext, target, isAutoPlay, resources, skipCardPileVisuals);
        countdown.ResetCount();
    }

    //This function prompts the user to discard for the countdown, and determines whether the card should play when the wrapper executes.
    private static async Task ResolveCountdownPrefix(CardModel cardModel, ICountdown countdown, PlayerChoiceContext choiceContext, Creature? target, bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals)
    {
        choiceContext.PushModel(cardModel);
        try
        {
            CardModel[] cardsDiscarded =
            [
                //This must be blocking, otherwise the action queue gets out of order.
                //Other players are able to queue up cards in the mean time, but those cards won't resolve.
                ..await CardSelectCmd.FromHandForDiscard(new BlockingPlayerChoiceContext(), cardModel.Owner,
                    new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt,
                        0, countdown.CurrentCount), null, cardModel)
            ];
            await CardCmd.Discard(choiceContext, cardsDiscarded);
            await countdown.DecrementCountAsync(cardsDiscarded.Length);
        }
        finally
        {
            choiceContext.PopModel(cardModel);
        }

        if (countdown.CurrentCount == 0)
        {
            //If the current countdown is 0, we execute, taking the branch allowing for normal execution.
            await cardModel.OnPlayWrapper(choiceContext, target, isAutoPlay, resources, skipCardPileVisuals);
            countdown.ResetCount();
        }
        else
        {
            //Otherwise, we will set the play count to 0 in the CountdownCanceller, and then run execution
            //Running the card through the queue with a play count of 0 is required to dequeue the card in multiplayer...for some reason
            CountdownCanceller.Cancel(countdown);
            await cardModel.OnPlayWrapper(choiceContext, target, isAutoPlay, resources, skipCardPileVisuals);
        }
    }
}