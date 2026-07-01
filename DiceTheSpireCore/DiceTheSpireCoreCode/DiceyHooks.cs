using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode;

public static class DiceyHooks
{
    public delegate void AfterCardCountsDownHandler(IRunState runState, CardModel countdownCard);

    public static event AfterCardCountsDownHandler? AfterCardCountsDown;

    internal static async Task OnAfterCardCountsDownAsync(IRunState runState, ICombatState? combatState, CardModel countdownCard)
    {
        AfterCardCountsDown?.Invoke(runState, countdownCard);
        foreach (IAfterCardCountsDownListener listener in runState.IterateHookListeners(combatState).OfType<IAfterCardCountsDownListener>())
        {
            await listener.AfterCardCountsDownAsync(runState, countdownCard);
        }
    }


    public delegate void ModifyPipOnPlayHandler(PlayerChoiceContext choiceContext, CardPlay cardPlay);

    public static event ModifyPipOnPlayHandler? ModifyPipOnPlay;

    internal static async Task OnModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ModifyPipOnPlay?.Invoke(choiceContext, cardPlay);
        foreach (IModifyPipOnPlayListener listener in cardPlay.Card.Owner.RunState
                     .IterateHookListeners(cardPlay.Card.Owner.Creature.CombatState)
                     .OfType<IModifyPipOnPlayListener>())
        {
            await listener.ModifyPipOnPlayAsync(choiceContext, cardPlay);
        }
    }

    public delegate void AfterKeywordAddedHandler(CardModel card, CardKeyword keyword);

    public static event AfterKeywordAddedHandler? AfterKeywordAdded;

    //Warning, no harmony patch has been set up for this function yet, so don't expect it to work unless you call it manually after adding the keyword.

    public static async Task OnKeywordAdded(CardModel card, CardKeyword keyword)
    {
        AfterKeywordAdded?.Invoke(card, keyword);
        if (card.RunState is null)
        {
            return;
        }

        foreach (IAfterKeywordAddedListener listener in card.RunState.IterateHookListeners(card.CombatState).OfType<IAfterKeywordAddedListener>())
        {
            await listener.AfterKeywordAddedAsync(card, keyword);
        }

    }

    public delegate void AfterBumpHandler(PlayerChoiceContext choiceContext, CardModel card, CardModel? newCard);

    public static event AfterBumpHandler? AfterBump;

    public static async Task OnAfterBumpAsync(PlayerChoiceContext choiceContext, CardModel card, CardModel? newCard)
    {
        AfterBump?.Invoke(choiceContext, card, newCard);
        if (card.RunState is null)
        {
            return;
        }

        foreach (IAfterBumpListener listener in card.Owner.RunState.IterateHookListeners(card.CombatState).OfType<IAfterBumpListener>())
        {
            await listener.AfterBumpAsync(choiceContext, card, newCard);
        }
    }

    public delegate void AfterNudgeHandler(PlayerChoiceContext choiceContext, CardModel card, bool wasExhausted);

    public static event AfterNudgeHandler? AfterNudge;

    public static async Task OnAfterNudgeAsync(PlayerChoiceContext choiceContext, CardModel card, bool wasExhausted)
    {
        AfterNudge?.Invoke(choiceContext, card, wasExhausted);
        if (card.RunState is null)
        {
            return;
        }

        foreach (IAfterNudgeListener listener in card.Owner.RunState.IterateHookListeners(card.CombatState).OfType<IAfterNudgeListener>())
        {
            await listener.AfterNudgeAsync(choiceContext, card, wasExhausted);
        }
    }
}