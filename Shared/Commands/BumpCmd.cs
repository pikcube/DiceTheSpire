using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Commands;

public static class BumpCmd
{
    public static async Task BumpAsync(PlayerChoiceContext choiceContext, CardModel instance)
    {
        if (instance.IsUpgradable)
        {
            CardCmd.Upgrade(instance);
            await DiceyHooks.OnAfterBumpAsync(choiceContext, instance, null);
        }
        else
        {
            CardModel newCard = instance.CanonicalInstance.ToMutable();
            newCard.Owner = null!;
            CardCmd.ClearAffliction(newCard);
            CardCmd.ClearEnchantment(newCard);
            instance.CombatState?.AddCard(newCard, instance.Owner);
            PileType newPileType = DefaultPile(instance.Pile?.Type);
            if (newPileType is PileType.Draw)
            {
                await CardPileCmd.AddGeneratedCardToCombat(newCard, PileType.Draw, instance.Owner, CardPilePosition.Random);
            }
            else
            {
                await CardPileCmd.AddGeneratedCardToCombat(newCard, newPileType, instance.Owner);
            }
            await DiceyHooks.OnAfterBumpAsync(choiceContext, instance, newCard);
        }
    }

    private static PileType DefaultPile(PileType? originType)
    {
        return PileType.Discard;
    }
}