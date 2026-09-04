using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Utility;

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

    //public static event AfterNudgeHandler? AfterNudge;

    //public static async Task OnAfterNudgeAsync(PlayerChoiceContext choiceContext, CardModel card, bool wasExhausted)
    //{
    //    AfterNudge?.Invoke(choiceContext, card, wasExhausted);
    //    RunState? state = RunManager.Instance.PrivatePropertyWrapper<RunManager, RunState>("State").Value;
    //    if (state is null)
    //    {
    //        return;
    //    }

    //    foreach (IAfterNudgeListener listener in state.IterateHookListeners(card.CombatState).OfType<IAfterNudgeListener>())
    //    {
    //        await listener.AfterNudgeAsync(choiceContext, card, wasExhausted);
    //    }
    //}

    public static async Task OnNudgeAsync(IRunState runState, CardModel card, int originalCost, int getAmountToSpend, NudgeDuration duration)
    {
        foreach (IAfterNudgeListener listener in runState.IterateHookListeners(card.CombatState).OfType<IAfterNudgeListener>())
        {
            await listener.AfterNudgeAsync(card, originalCost, getAmountToSpend, duration);
        }
    }

    public static async Task OnFlipAsync(IRunState runState, CardModel card, int originalCost, int getAmountToSpend, FlipDuration duration)
    {
        foreach (IAfterFlipListener listener in runState.IterateHookListeners(card.CombatState).OfType<IAfterFlipListener>())
        {
            await listener.AfterFlipAsync(card, originalCost, getAmountToSpend, duration);
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

    public static bool OnModifyUnplayableBehavior(IRunState runState, CardModel card)
    {
        return runState.IterateHookListeners(card.CombatState)
            .OfType<IModifyUnplayableBehaviorListener>()
            .Any(listener => listener.ModifyUnplayableBehavior(card));
    }

    public static bool OnModifyTargetType(IRunState runState, CardModel card, ref TargetType targetType)
    {
        bool isModified = false;
        foreach (IModifyUnplayableBehaviorListener listener in runState.IterateHookListeners(card.CombatState).OfType<IModifyUnplayableBehaviorListener>())
        {
            isModified = listener.TryModifyTargetType(card, ref targetType);
            if (isModified)
            {
                return true;
            }
        }
        return isModified;
    }

    public static bool TryModifyUnplayableOnPlay(IRunState runState, CardModel card, out Func<PlayerChoiceContext, CardPlay, Task> task)
    {
        bool isModified = false;
        task = (_, _) => Task.CompletedTask;
        foreach (IModifyUnplayableBehaviorListener listener in runState.IterateHookListeners(card.CombatState).OfType<IModifyUnplayableBehaviorListener>())
        {
            isModified = listener.TryModifyOnPlay(card, ref task);
            if (isModified)
            {
                return true;
            }
        }
        return isModified;
    }

    public static async Task OnRerollAsync(IRunState runState, CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
    {
        foreach (IAfterRerollListener listener in runState.IterateHookListeners(card.CombatState).OfType<IAfterRerollListener>())
        {
            await listener.AfterRerollAsync(card, isFixed, originalCost, getAmountToSpend, duration);
        }
    }

    public static void ModifyFuryPlayCount(IRunState runState, FuryPower furyPower, CardModel card, ref int furyCount)
    {
        foreach (IModifyFuryPlayCountListener listener in runState.IterateHookListeners(card.CombatState)
                     .OfType<IModifyFuryPlayCountListener>())
        {
            listener.ModifyFuryPlayCount(furyPower, card, ref furyCount);
        }
    }
}