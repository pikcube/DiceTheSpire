using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class PlayerExtensions
{
    extension(Player instance)
    {
        public async Task InspectAsync(PlayerChoiceContext choiceContext, int cards, Func<Task>? onBlinkFunc = null)
        {
            CardModel[] topCards = [.. PileType.Draw.GetPile(instance).Cards.Take(cards)];

            if (topCards.Length == 0)
            {
                return;
            }

            CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_BLINK"), 0, topCards.Length);

            CardModel[] selectedCards = [.. await CardSelectCmd.FromSimpleGrid(choiceContext, topCards, instance, prefs)];

            foreach (CardModel card in selectedCards)
            {
                await card.BlinkAsync(choiceContext);
                if (onBlinkFunc is not null)
                {
                    await onBlinkFunc();
                }
            }

            foreach (IOnInspectListener listener in instance.RunState.IterateHookListeners(instance.Creature.CombatState).OfType<IOnInspectListener>())
            {
                await listener.OnInspectAsync(choiceContext, cards, selectedCards, instance);
            }
        }
    }
}

public interface IOnInspectListener
{
    public Task OnInspectAsync(PlayerChoiceContext choiceContext, int cards, CardModel[] selectedCards, Player inspector);
}