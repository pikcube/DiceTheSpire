using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode;

public static class DiceyHooks
{
    public static async Task OnTurnEndInHand(CardModel card, IRunState runState, ICombatState combatState)
    {
        foreach (IOnTurnEndInHandListener listener in runState.IterateHookListeners(combatState).OfType<IOnTurnEndInHandListener>())
        {
            await listener.AfterTurnEndInHandEffectAsync(card);
        }
    }
}

public interface IOnTurnEndInHandListener
{
    public Task AfterTurnEndInHandEffectAsync(CardModel card);
}