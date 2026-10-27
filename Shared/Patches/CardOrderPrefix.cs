using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(NPlayerHand), nameof(NPlayerHand.Add))]
public static class CardOrderPrefix
{
    public static void Prefix(NCard card, ref int index)
    {
        CardModel? cardModel = card.Model;
        CardPile? pile = cardModel?.Pile;

        if (pile?.Type is not PileType.Hand || index != -1)
        {
            return;
        }

        index = pile.Cards.IndexOf(cardModel);
    }
}