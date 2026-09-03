using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Commands;

public static class NudgeCmd
{
    public static async Task NudgeAsync(CardModel card, NudgeDuration duration)
    {
        if (card.EnergyCost.CostsX)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(card.RunState);
        int originalCost = card.EnergyCost.GetAmountToSpend();

        int nextEnergyCost = NextEnergyCost(card);

        switch (duration)
        {
            case NudgeDuration.Combat:
                card.EnergyCost.SetThisCombat(nextEnergyCost);
                break;
            case NudgeDuration.UntilPlayed:
                card.EnergyCost.SetUntilPlayed(nextEnergyCost);
                break;
            case NudgeDuration.UntilEndOfTurn:
                card.EnergyCost.SetThisTurn(nextEnergyCost);
                break;
            case NudgeDuration.UntilEndOfTurnOrPlayed:
                card.EnergyCost.SetThisTurnOrUntilPlayed(nextEnergyCost);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(duration), duration, null);
        }

        await DiceyHooks.OnNudgeAsync(card.RunState, card, originalCost, card.EnergyCost.GetAmountToSpend(), duration);
    }

    private static int NextEnergyCost(CardModel card)
    {
        return card.EnergyCost.CostsX 
            ? throw new ArgumentException("Cannot nudge X-Cost card", nameof(card)) 
            : Math.Max(card.EnergyCost.GetAmountToSpend() - 1, 0);
    }
}

public enum NudgeDuration
{
    Combat = 0,
    UntilPlayed = 1,
    UntilEndOfTurn = 2,
    UntilEndOfTurnOrPlayed = 3,
}