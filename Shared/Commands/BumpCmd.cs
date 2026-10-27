using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Commands;

public static class BumpCmd
{
    public static async Task BumpAsync(PlayerChoiceContext choiceContext, CardModel instance)
    {
        if (instance.RunState is null || instance.CombatState is null || instance.Pile is null)
        {
            return;
        }
        if (instance.IsUpgradable)
        {
            CardCmd.Upgrade(instance);
            await DiceyHooks.OnAfterBumpAsync(instance.RunState, instance.CombatState, choiceContext, instance, null);
            UnstuckIfNecessary(instance);
        }
        else
        {
            CardModel newCard = instance.StrongMutableClone();
            newCard.DowngradeInternal();
            CardCmd.ClearAffliction(newCard);
            CardCmd.ClearEnchantment(newCard);
            newCard.Owner = null!;
            instance.CombatState?.AddCard(newCard, instance.Owner);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(newCard, PileType.Discard, instance.Owner));
            await DiceyHooks.OnAfterBumpAsync(instance.RunState, instance.CombatState, choiceContext, instance, newCard);
        }
    }

    private static void UnstuckIfNecessary(CardModel card)
    {
        //Verify that the card belongs in the local player's hand
        if (card.Pile?.Type != PileType.Hand || !LocalContext.IsMe(card.Owner))
        {
            return;
        }

        NPlayerHand? hand = NPlayerHand.Instance;
        
        //Verify that the hand exists. If it doesn't, god can no longer save you
        if (hand == null)
        {
            return;
        }

        //If the card has an existing holder, it's not stuck
        if (hand.GetCardHolder(card) is not null)
        {
            return;
        }

        //Find the card on the table
        NCard? nCard = NCard.FindOnTable(card);

        //If we find the card, add it to the hand directly
        if (nCard is not null)
        {
            hand.Add(nCard, card.Pile?.Cards.IndexOf(card) ?? -1);
            return;
        }
        
        //Fallback option in case an actual tween is needed...somehow
        CardPileCmd.GetTweenForCardsChangingPiles([new CardPileAddResult
        {
            cardAdded = card,
            modifyingModels = null,
            oldPile = card.Pile,
            success = true,
            targetPile = card.Pile?.Type ?? PileType.Hand
        }], true);
    }
}