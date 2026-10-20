using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.RemoveFromDeck), typeof(IReadOnlyList<CardModel>), typeof(bool))]
public static class RemoveFromDeckPatches
{
    private static List<(CardModel, IInterruptRemovalListener)> RemovalsInterrupted { get; set; } = [];
    public static void Prefix(ref IReadOnlyList<CardModel> cards)
    {
        lock (RemovalsInterrupted)
        {
            cards = [..cards.Where(c =>
            {
                if (c.RunState is null)
                {
                    return true;
                }
                bool isRemovalAllowed = !DiceyHooks.ShouldInterruptRemoval(c.RunState, c.CombatState, c, out IInterruptRemovalListener? listener);
                if (!isRemovalAllowed && listener is not null)
                {
                    RemovalsInterrupted.Add((c, listener));
                }

                return isRemovalAllowed;
            })];
        }
    }

    public static async Task Postfix(Task __result)
    {
        await __result;
        List<(CardModel, IInterruptRemovalListener)> removals = [];
        lock (RemovalsInterrupted)
        {
            removals.AddRange(RemovalsInterrupted);
            RemovalsInterrupted.Clear();
        }
        foreach ((CardModel card, IInterruptRemovalListener listener) in removals)
        {
            await listener.AfterRemovalInterrupted(card);
        }
    }
}