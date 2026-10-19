using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Commands;

public static class BumpCmd
{
    public static async Task BumpAsync(PlayerChoiceContext choiceContext, CardModel instance)
    {
        if (instance.RunState is null || instance.CombatState is null)
        {
            return;
        }
        if (instance.IsUpgradable)
        {
            CardCmd.Upgrade(instance);
            await DiceyHooks.OnAfterBumpAsync(instance.RunState, instance.CombatState, choiceContext, instance, null);
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
}