using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Commands;

public static class FlipCmd
{
    public static async Task FlipAsync(CardModel card, FlipDuration duration)
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
            case FlipDuration.Combat:
                card.EnergyCost.SetThisCombat(nextEnergyCost);
                break;
            case FlipDuration.UntilPlayed:
                card.EnergyCost.SetUntilPlayed(nextEnergyCost);
                break;
            case FlipDuration.UntilEndOfTurn:
                card.EnergyCost.SetThisTurn(nextEnergyCost);
                break;
            case FlipDuration.UntilEndOfTurnOrPlayed:
                card.EnergyCost.SetThisTurnOrUntilPlayed(nextEnergyCost);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(duration), duration, null);
        }

        await DiceyHooks.OnFlipAsync(card.RunState, card, originalCost, card.EnergyCost.GetAmountToSpend(), duration);
    }

    private static int NextEnergyCost(CardModel card)
    {
        return card.EnergyCost.CostsX 
            ? throw new ArgumentException("Cannot flip X-Cost card", nameof(card)) 
            : Math.Max(3 - card.EnergyCost.GetAmountToSpend(), 0);
    }
}

public enum FlipDuration
{
    Combat = 0,
    UntilPlayed = 1,
    UntilEndOfTurn = 2,
    UntilEndOfTurnOrPlayed = 3,
}