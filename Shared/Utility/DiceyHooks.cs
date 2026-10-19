using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Utility;

public static class DiceyHooks
{ 
    internal static async Task OnAfterCardCountsDownAsync(IRunState runState, ICombatState? combatState, CardModel countdownCard)
    {
        foreach (IAfterCardCountsDownListener listener in runState.IterateHookListeners(combatState).OfType<IAfterCardCountsDownListener>())
        {
            await listener.AfterCardCountsDownAsync(runState, countdownCard);
        }
    }

    internal static async Task OnModifyPipOnPlayAsync(IRunState runState, ICombatState? combatState, PlayerChoiceContext choiceContext, CardPlay cardPlay, Pip pip)
    {
        foreach (IModifyPipOnPlayListener listener in runState.IterateHookListeners(combatState)
                     .OfType<IModifyPipOnPlayListener>())
        {
            await listener.ModifyPipOnPlayAsync(choiceContext, cardPlay, pip);
        }
    }
    internal static IEnumerable<string> ModifyPipDescription(IRunState runState, ICombatState? combatState, Pip pip)
    {
        return runState.IterateHookListeners(combatState)
            .OfType<IModifyPipOnPlayListener>()
            .Where(listener => listener.ShouldModify(pip))
            .Select(listener => (listener.PipDescription.GetFormattedText()));
    }

    internal static IEnumerable<IHoverTip> ModifyPipHoverTips(IRunState runState, ICombatState? combatState, Pip pip)
    {
        return runState.IterateHookListeners(combatState)
            .OfType<IModifyPipOnPlayListener>()
            .Where(listener => listener.ShouldModify(pip))
            .SelectMany(listener => listener.PipHoverTips);
    }

    internal static async Task OnAfterBumpAsync(IRunState runState, ICombatState? combatState, PlayerChoiceContext choiceContext, CardModel card, CardModel? newCard)
    {
        foreach (IAfterBumpListener listener in runState.IterateHookListeners(combatState).OfType<IAfterBumpListener>())
        {
            await listener.AfterBumpAsync(choiceContext, card, newCard);
        }
    }

    internal static async Task OnNudgeAsync(IRunState runState, ICombatState? combatState, CardModel card, int originalCost, int getAmountToSpend, NudgeDuration duration)
    {
        foreach (IAfterNudgeListener listener in runState.IterateHookListeners(combatState).OfType<IAfterNudgeListener>())
        {
            await listener.AfterNudgeAsync(card, originalCost, getAmountToSpend, duration);
        }
    }

    internal static async Task OnFlipAsync(IRunState runState, ICombatState? combatState, CardModel card, int originalCost, int getAmountToSpend, FlipDuration duration)
    {
        foreach (IAfterFlipListener listener in runState.IterateHookListeners(combatState).OfType<IAfterFlipListener>())
        {
            await listener.AfterFlipAsync(card, originalCost, getAmountToSpend, duration);
        }
    }

    internal static void OnModifyRerollRange(IRunState runState, ICombatState? combatState, CardModel card, ref int minimum, ref int maximum)
    {
        foreach (IModifyRerollListener listener in runState.IterateHookListeners(combatState).OfType<IModifyRerollListener>())
        {
            listener.ModifyRerollRange(card, ref minimum, ref maximum);
        }
    }

    internal static void OnModifyScrapPriority(IRunState runState, ICombatState? combatState, Player player, ref List<CardModel> scrapCards, ref List<CardModel> otherCards)
    {
        foreach (IModifyScrapPriorityListener listener in runState.IterateHookListeners(combatState).OfType<IModifyScrapPriorityListener>())
        {
            listener.ModifyPriority(player, ref scrapCards, ref otherCards);
        }
    }

    internal static bool OnModifyUnplayableBehavior(IRunState runState, ICombatState? combatState, CardModel card)
    {
        return runState.IterateHookListeners(combatState)
            .OfType<IModifyUnplayableBehaviorListener>()
            .Any(listener => listener.ModifyUnplayableBehavior(card));
    }

    internal static bool OnModifyTargetType(IRunState runState, ICombatState? combatState, CardModel card, ref TargetType targetType)
    {
        bool isModified = false;
        foreach (IModifyUnplayableBehaviorListener listener in runState.IterateHookListeners(combatState).OfType<IModifyUnplayableBehaviorListener>())
        {
            isModified = listener.TryModifyTargetType(card, ref targetType);
            if (isModified)
            {
                return true;
            }
        }
        return isModified;
    }

    internal static bool TryModifyUnplayableOnPlay(IRunState runState, ICombatState? combatState, CardModel card, out Func<PlayerChoiceContext, CardPlay, Task> task)
    {
        bool isModified = false;
        task = (_, _) => Task.CompletedTask;
        foreach (IModifyUnplayableBehaviorListener listener in runState.IterateHookListeners(combatState).OfType<IModifyUnplayableBehaviorListener>())
        {
            isModified = listener.TryModifyOnPlay(card, ref task);
            if (isModified)
            {
                return true;
            }
        }
        return isModified;
    }

    internal static async Task OnRerollAsync(IRunState runState, ICombatState? combatState, PlayerChoiceContext choiceContext, CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
    {
        foreach (IAfterRerollListener listener in runState.IterateHookListeners(combatState).OfType<IAfterRerollListener>())
        {
            await listener.AfterRerollAsync(choiceContext, card, isFixed, originalCost, getAmountToSpend, duration);
        }
    }

    internal static async Task OnShockAsync(IRunState runState, ICombatState? combatState, PlayerChoiceContext choiceContext, CardModel card)
    {
        foreach (IOnShockListener listener in runState.IterateHookListeners(combatState).OfType<IOnShockListener>())
        {
            await listener.AfterCardShockedAsync(choiceContext, card);
        }
    }

    internal static int ModifCountdownCost(ICombatState combatState, ICountdown countdown, CardModel cardModel, int cost)
    {
        return combatState.IterateHookListeners().OfType<ICountdownCostListener>()
            .Aggregate(cost, (current, listener) => listener.ModifyCost(countdown, cardModel, current));
    }

    internal static LocString ModifyRelicFlavor(LocString locString, RelicModel relicModel)
    {
        if (relicModel is IModifyFlavorRelic listener)
        {
            listener.ModifyFlavor(ref locString);
        }

        return locString;
    }

    internal static bool ShouldInterruptRemoval(IRunState runState, ICombatState? combatState, CardModel card,
        out IInterruptRemovalListener? interruptRemovalListener)
    {
        interruptRemovalListener = null;

        foreach (IInterruptRemovalListener listener in runState.IterateHookListeners(combatState)
                     .OfType<IInterruptRemovalListener>())
        {
            if (!listener.TryInterruptRemoval(card))
            {
                continue;
            }

            interruptRemovalListener = listener;
            return true;
        }

        return false;
    }

    internal static bool ShouldIgnoreMerchantCost(IRunState runState, ICombatState? combatState, MerchantEntry entry, MerchantInventory? inventory, out IIgnoreMerchantCostListener? ignoreMerchantCostListener)
    {
        ignoreMerchantCostListener = null;
        foreach (IIgnoreMerchantCostListener listener in runState.IterateHookListeners(combatState)
                     .OfType<IIgnoreMerchantCostListener>())
        {
            if (!listener.TryIgnoreCost(entry, inventory))
            {
                continue;
            }

            ignoreMerchantCostListener = listener;
            return true;
        }

        return false;
    }

    internal static async Task BetterAfterRestingAsync(IRunState runState, ICombatState? combatState, Player player)
    {
        foreach (IAfterRestingListener listener in runState.IterateHookListeners(combatState)
                     .OfType<IAfterRestingListener>())
        {
            await listener.BetterAfterRestingAsync(player);
        }
    }
}