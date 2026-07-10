using DiceTheSpireCore.DiceTheSpireCoreCode.Commands;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using Godot;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

public static class RerollPatches
{
    [HarmonyPatch(typeof(ConfusedPower), "AfterCardDrawn")]
    public static class ConfusedPatch
    {
        public static bool Prefix(ref Task __result, ConfusedPower __instance, CardModel card)
        {
            if (card is not IRangeCard || card.Owner != __instance.Owner.Player || card.EnergyCost.Canonical < 0)
            {
                return true;
            }

            RerollCmd.Reroll(card, RerollDuration.Combat);

            __result = Task.CompletedTask;
            return false;
        }
    }

    [HarmonyPatch(typeof(Slither), "AfterCardDrawn")]
    public static class SlitherPatch
    {
        public static bool Prefix(ref Task __result, Slither __instance, CardModel card)
        {
            if (card is not IRangeCard || card != __instance.Card || __instance.Card.Pile?.Type != PileType.Hand)
            {
                return true;
            }

            RerollCmd.Reroll(card, RerollDuration.Combat);
            __result = Task.CompletedTask;
            return false;
        }
    }

    [HarmonyPatch(typeof(SneckoOil), "OnUse")]
    public static class SneckoOilPatch
    {
        public static bool Prefix(ref Task __result, SneckoOil __instance, PlayerChoiceContext choiceContext,
            Creature? target)
        {
            __result = DoSneckoOil(__instance, choiceContext, target);
            return false;
        }

        private static async Task DoSneckoOil(SneckoOil sneckoOil, PlayerChoiceContext choiceContext, Creature? target)
        {
            if (target?.Player is null)
            {
                return;
            }
            NCombatRoom.Instance?.PlaySplashVfx(target, new Color("6ec46f"));
            await CardPileCmd.Draw(choiceContext, sneckoOil.DynamicVars.Cards.BaseValue, target.Player);
            foreach (CardModel card in PileType.Hand.GetPile(target.Player).Cards.Where(c => !c.EnergyCost.CostsX))
            {
                RerollCmd.Reroll(card, RerollDuration.UntilEndOfTurnOrPlayed);
            }
        }
    }
}