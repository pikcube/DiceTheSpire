using DiceTheSpireCore.DiceTheSpireCoreCode.DynamicVars;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Commands;

public static class RerollCmd
{
    public static async Task RerollAsync(CardModel card, RerollDuration duration)
    {
        ArgumentNullException.ThrowIfNull(card.RunState);
        int originalCost = card.EnergyCost.GetAmountToSpend();

        int nextEnergyCost = NextEnergyCost(card, card.RunState, out bool isFixed);

        switch (duration)
        {
            case RerollDuration.Combat:
                card.EnergyCost.SetThisCombat(nextEnergyCost);
                break;
            case RerollDuration.UntilPlayed:
                card.EnergyCost.SetUntilPlayed(nextEnergyCost);
                break;
            case RerollDuration.UntilEndOfTurn:
                card.EnergyCost.SetThisTurn(nextEnergyCost);
                break;
            case RerollDuration.UntilEndOfTurnOrPlayed:
                card.EnergyCost.SetThisTurnOrUntilPlayed(nextEnergyCost);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(duration), duration, null);
        }

        if (!isFixed)
        {
            NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        }

        await DiceyHooks.OnRerollAsync(card.RunState, card, isFixed, originalCost, card.EnergyCost.GetAmountToSpend(), duration);
    }

    private static int NextEnergyCost(CardModel card, IRunState runState, out bool isFixed)
    {
        int minimum = 0;
        int maximum = 3;
        
        if (RangeVars.TryGet(card, out int min, out int max))
        {
            minimum = min;
            maximum = max;
        }

        DiceyHooks.OnModifyRerollRange(runState, card, ref minimum, ref maximum);

        isFixed = minimum == maximum;

        if (minimum > maximum)
        {
            (minimum, maximum) = (maximum, minimum);
        }

        return runState.Rng.CombatEnergyCosts.NextInt(minimum, maximum + 1);

    }
}

public enum RerollDuration
{
    Combat = 0,
    UntilPlayed = 1,
    UntilEndOfTurn = 2,
    UntilEndOfTurnOrPlayed = 3,
}