using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace DiceTheSpireCore.DiceTheSpireCoreCode;

public static class RerollCmd
{
    public static async Task RerollAsync(CardModel card, bool isPermanent)
    {
        int nextEnergyCost = NextEnergyCost(card, out bool isFixed);

        if (isPermanent)
        {
            card.EnergyCost.SetThisCombat(nextEnergyCost);
        }
        else
        {
            card.EnergyCost.SetThisTurnOrUntilPlayed(nextEnergyCost);
        }

        if (isFixed)
        {
            return;
        }

        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        await Cmd.Wait(0.5f);
    }

    private static int NextEnergyCost(CardModel card, out bool isFixed)
    {
        if (card is not IRangeCard range)
        {
            isFixed = false;
            return card.RunState?.Rng.CombatEnergyCosts.NextInt(4) ?? 0;
        }

        if (range.MaximumCost == range.MinimumCost)
        {
            isFixed = true;
            return range.MinimumCost;
        }

        isFixed = false;
        return card.RunState?.Rng.CombatEnergyCosts.NextInt(range.MinimumCost, range.MaximumCost + 1) ?? range.MinimumCost;

    }
}