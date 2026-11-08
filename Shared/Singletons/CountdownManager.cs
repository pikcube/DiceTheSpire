using BaseLib.Abstracts;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Singletons;

public class CountdownManager() : CustomSingletonModel(HookType.Combat)
{
    public override Task BeforeCardAutoPlayed(CardModel card, Creature? target, AutoPlayType type)
    {
        ArgumentNullException.ThrowIfNull(card.CombatState);
        if (card is not ICountdown countdown)
        {
            return Task.CompletedTask;
        }

        countdown.CountSpent ??= 0;
        countdown.CountValue ??= countdown.HasCountdownCostX ? Hook.ModifyXValue(card.CombatState, card, countdown.Count) : countdown.Count;

        return Task.CompletedTask;
    }

    public override Task AfterCardChangedPilesLate(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card is not ICountdown countdown || oldPileType != PileType.Play)
        {
            return Task.CompletedTask;
        }

        countdown.CountSpent = null;
        countdown.CountValue = null;

        return Task.CompletedTask;
    }
}