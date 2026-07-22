using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;

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

    public delegate void AfterCardShockedHandler(CardModel card);

    public static event AfterCardShockedHandler? AfterCardShocked;


    public static async Task OnCardShocked(PlayerChoiceContext choiceContext, ShockPower shock, CardModel card)
    {
        AfterCardShocked?.Invoke(card);
        if (card.RunState is null)
        {
            return;
        }

        foreach (IAfterCardShockedListener listener in card.RunState.IterateHookListeners(card.CombatState).OfType<IAfterCardShockedListener>())
        {
            await listener.AfterCardShockedAsync(choiceContext, shock, card);
        }

    }

    public delegate void AfterBumpHandler(PlayerChoiceContext choiceContext, CardModel card, CardModel? newCard);

    public static event AfterBumpHandler? AfterBump;

    public static async Task OnAfterBumpAsync(PlayerChoiceContext choiceContext, CardModel card, CardModel? newCard)
    {
        AfterBump?.Invoke(choiceContext, card, newCard);
        RunState? state = RunManager.Instance.PrivatePropertyWrapper<RunManager, RunState>("State").Value;
        if (state is null)
        {
            return;
        }

        foreach (IAfterBumpListener listener in state.IterateHookListeners(card.CombatState).OfType<IAfterBumpListener>())
        {
            await listener.AfterBumpAsync(choiceContext, card, newCard);
        }
    }

    public delegate void AfterNudgeHandler(PlayerChoiceContext choiceContext, CardModel card, bool wasExhausted);

    public static event AfterNudgeHandler? AfterNudge;

    public static async Task OnAfterNudgeAsync(PlayerChoiceContext choiceContext, CardModel card, bool wasExhausted)
    {
        AfterNudge?.Invoke(choiceContext, card, wasExhausted);
        RunState? state = RunManager.Instance.PrivatePropertyWrapper<RunManager, RunState>("State").Value;
        if (state is null)
        {
            return;
        }

        foreach (IAfterNudgeListener listener in state.IterateHookListeners(card.CombatState).OfType<IAfterNudgeListener>())
        {
            await listener.AfterNudgeAsync(choiceContext, card, wasExhausted);
        }
    }

    public static void OnModifyRerollRange(IRunState runState, CardModel card, ref int minimum, ref int maximum)
    {
        foreach (IModifyRerollListener listener in runState.IterateHookListeners(card.CombatState).OfType<IModifyRerollListener>())
        {
            listener.ModifyRerollRange(card, ref minimum, ref maximum);
        }
    }

    public static void OnModifyScrapPriority(IRunState runState, Player player, ref List<CardModel> scrapCards, ref List<CardModel> otherCards)
    {
        foreach (IModifyScrapPriorityListener listener in runState.IterateHookListeners(null).OfType<IModifyScrapPriorityListener>())
        {
            listener.ModifyPriority(player, ref scrapCards, ref otherCards);
        }
    }

    public static bool OnModifyUnplayableBehavior(IRunState runState, CardModel card, out Func<PlayerChoiceContext, CardPlay, Task>? newOnPlay)
    {
        bool isNewOnPlayUsed = false;
        newOnPlay = null;
        foreach (IModifyUnplayableBehaviorListener listener in runState.IterateHookListeners(card.CombatState).OfType<IModifyUnplayableBehaviorListener>())
        {
            isNewOnPlayUsed = listener.ModifyUnplayableBehavior(card, ref newOnPlay);
            if (isNewOnPlayUsed)
            {
                return true;
            }
        }

        return isNewOnPlayUsed;
    }
}