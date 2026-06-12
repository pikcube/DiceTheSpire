using DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;
using MegaCrit.Sts2.Core.Models;
using RunState = MegaCrit.Sts2.Core.Runs.RunState;

namespace DiceTheSpireCore.DiceTheSpireCoreCode;

public static class DiceyHooks
{
    public delegate void AfterCardCountsDownHandler(RunState runState, CardModel countdownCard);

    public static event AfterCardCountsDownHandler? AfterCardCountsDown;

    internal static async Task OnAfterCardCountsDownAsync(RunState runState, CardModel countdownCard)
    {
        AfterCardCountsDown?.Invoke(runState, countdownCard);
        foreach (IAfterCardCountsDownListener listener in runState.IterateHookListeners(countdownCard.Owner.Creature.CombatState).OfType<IAfterCardCountsDownListener>())
        {
            await listener.AfterCardCountsDownAsync(runState, countdownCard);
        }
    }
}