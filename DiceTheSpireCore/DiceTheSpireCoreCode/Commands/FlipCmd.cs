using DiceTheSpireCore.DiceTheSpireCoreCode.DynamicVars;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Commands;

public static class FlipCmd
{
    public static async Task FlipAsync(CardModel card, FlipDuration duration)
    {
        ArgumentNullException.ThrowIfNull(card.RunState);
        int originalCost = card.EnergyCost.GetAmountToSpend();

        int nextEnergyCost = NextEnergyCost(card, card.RunState);

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

    private static int NextEnergyCost(CardModel card, IRunState runState)
    {
        if (card.EnergyCost == null) 
            return 0;
        if (card.EnergyCost.CostsX)
            return -1;
        if (card.EnergyCost.GetAmountToSpend() == 0)
            return 3;
        if (card.EnergyCost.GetAmountToSpend() == 1)
            return 2;
        if (card.EnergyCost.GetAmountToSpend() == 2)
            return 1;
        if (card.EnergyCost.GetAmountToSpend() >= 3)
            return 0;

        return 0;

    }
}

public enum FlipDuration
{
    Combat = 0,
    UntilPlayed = 1,
    UntilEndOfTurn = 2,
    UntilEndOfTurnOrPlayed = 3,
}





