using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Extensions;

public static class CombatHistoryExtensions
{
    extension(CombatHistory instance)
    {
        public void AddShockEntry(CardModel card)
        {
            if (card.CombatState == null)
            {
                return;
            }

            typeof(CombatHistory).DeclaredMethod("Add")
                .Invoke(instance, [card.CombatState, new CardShockedEntry(card, card.CombatState, instance)]);
        }
    }
}