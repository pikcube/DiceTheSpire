using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Commands;

public static class RerollCmd
{
    public static void Reroll(CardModel card, RerollDuration duration)
    {
        ArgumentNullException.ThrowIfNull(card.RunState);

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

        if (isFixed)
        {
            return;
        }

        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
    }

    private static int NextEnergyCost(CardModel card, IRunState runState, out bool isFixed)
    {
        int minimum = 0;
        int maximum = 3;
        isFixed = false;
        
        if (card is IRangeCard range)
        {
            minimum = range.MinimumCost;
            maximum = range.MaximumCost;
        }

        DiceyHooks.OnModifyRerollRange(runState, card, ref minimum, ref maximum);

        isFixed = minimum == maximum;

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