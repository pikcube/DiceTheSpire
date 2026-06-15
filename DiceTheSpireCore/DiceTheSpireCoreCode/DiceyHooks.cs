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
                     .IterateHookListeners(cardPlay.Card.Owner.Creature
                     .CombatState).OfType<IModifyPipOnPlayListener>().Where(l => l.Owner == cardPlay.Card.Owner))
        {
            await listener.ModifyPipOnPlayAsync(choiceContext, cardPlay);
        }
    }
}