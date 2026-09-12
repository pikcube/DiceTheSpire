using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Keywords;

public class ShockModel() : CustomSingletonModel(HookType.Combat)
{
    private static List<CardModel> ShouldShockList { get; } = [];

    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Shock = 0;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Shocked = 0;

    /// <inheritdoc />
    public override Task BeforeRoomEntered(AbstractRoom room)
    {
        ShouldShockList.Clear();
        return Task.CompletedTask;
    }

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card.Keywords.Contains(Shock) || card.ShouldShockOnNextPlay)
        {
            return new CardLocation(card.Owner, PileType.Exhaust, CardPilePosition.Bottom);
        }


        return cardLocation;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!cardPlay.Card.Keywords.Contains(Shock) && !cardPlay.Card.ShouldShockOnNextPlay)
        {
            return;
        }

        cardPlay.Card.AddPurpleKeyword(Shocked);

        await DiceyHooks.OnShockAsync(choiceContext, cardPlay.Card);
        cardPlay.Card.ShouldShockOnNextPlay = false;
    }

    public static async Task ShockCardAsync(PlayerChoiceContext choiceContext, CardModel card, bool skipVisuals = false)
    {
        await card.ExhaustAsync(choiceContext, false, skipVisuals);
        card.AddPurpleKeyword(Shocked);
        await DiceyHooks.OnShockAsync(choiceContext, card);
    }

    internal static bool ShouldShock<T>(T instance) where T : CardModel
    {
        return !instance.ExhaustOnNextPlay && ShouldShockList.Any(c => c == instance);
    }

    internal static void SetShouldShock<T>(T instance, bool value) where T : CardModel
    {
        if (value)
        {
            ShouldShockList.Add(instance);
        }
        else
        {
            ShouldShockList.Remove(instance);
        }
    }

    public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Keywords.Contains(Shocked) && card.Pile?.Type != PileType.Exhaust)
        {
            card.RemoveKeyword(Shocked);
        }

        return Task.CompletedTask;
    }

    public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.PlayerCombatState is null)
        {
            return;
        }

        List<CardModel> shockedCards = [.. player.PlayerCombatState.AllCards.Where(c => c.Keywords.Contains(Shocked))];
        List<Task> tasks = [];
        foreach (CardModel card in shockedCards)
        {
            card.RemoveKeyword(Shocked);
            if (card.Pile?.Type != PileType.Exhaust)
            {
                continue;
            }

            tasks.Add(CardPileCmd.Add(card, PileType.Hand));
        }
        tasks.Add(Task.Delay(TimeSpan.FromSeconds(0.15)));
        await Task.WhenAll(tasks);
    }

}