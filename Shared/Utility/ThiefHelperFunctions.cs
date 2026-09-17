using DiceTheSpire.Inventor;
using DiceTheSpire.Thief;
using DiceTheSpire.Warrior;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Utility;

public static class ThiefHelperFunctions
{
    public static IEnumerable<CardPoolModel> BaseCharacterCardPools =>
    [
        ModelDb.CardPool<IroncladCardPool>(), ModelDb.CardPool<SilentCardPool>(), ModelDb.CardPool<RegentCardPool>(),
        ModelDb.CardPool<NecrobinderCardPool>(), ModelDb.CardPool<DefectCardPool>()
    ];
    

    public static IEnumerable<CardPoolModel> DiceyNonThiefCardPools =>
    [
        ModelDb.CardPool<TheWarriorCardPool>(), ModelDb.CardPool<TheInventorCardPool>()
    ];

    public static IEnumerable<CardPoolModel> NonThiefCharacterPools =>
        ModelDb.AllCharacterCardPools.Where(pool => pool is not TheThiefCardPool);

    /// <summary>
    /// Generates distinct cards of the provided character card pools for use in combat.
    /// </summary>
    /// <param name="player">The player receiving the generated cards.</param>
    /// <param name="cardPools">The card pools to pull cards from.</param>
    /// <param name="count">The number of cards to generate.</param>
    /// <param name="rng">Rng used to randomize the Card Pools.</param>
    /// <param name="filter">If a filter is provided, will only generate cards where the filter returns true.</param>
    /// <returns>The generated CardModels</returns>
    public static IEnumerable<CardModel> GetDistinctOfClassForCombat(Player player, IEnumerable<CardPoolModel> cardPools, int count, Rng rng, Func<CardModel, bool>? filter = null)
    {
        return CardFactory.GetDistinctForCombat(player, cardPools.SelectMany(CollectionSelector), count, rng);

        
        IEnumerable<CardModel> CollectionSelector(CardPoolModel cardPool)
        {
            IEnumerable<CardModel> cardModels = cardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint);
            if (filter is not null)
            {
                cardModels = cardModels.Where(filter);
            }

            return cardModels;
        }
    }

    /// <summary>
    /// Generates cards of the provided character card pools for reward screens.
    /// </summary>
    /// <param name="player">The player receiving the rewards.</param>
    /// <param name="cardPools">The card pools to pull cards from.</param>
    /// <param name="rarityOdds">The rarity odds for generated cards. If applying a filter, use uniform odds</param>
    /// <param name="count">The number of cards to generate.</param>
    /// <param name="filter">If a filter is provided, will only generate cards where the filter returns true.</param>
    /// <returns>The CardCreationResult (must still be added to the reward list)</returns>
    public static IEnumerable<CardCreationResult> GetCardsOfClassForReward(Player player, IEnumerable<CardPoolModel> cardPools, int count, CardRarityOddsType rarityOdds, Func<CardModel, bool>? filter = null)
    {
        CardCreationOptions options = new(cardPools, CardCreationSource.Other, rarityOdds, filter);
        //Prevents a stack overflow from Stickyfingers modifying itself repeatedly
        options.WithFlags(CardCreationFlags.NoModifyHooks);
        return CardFactory.CreateForReward(player, count, options).Take(count);
    }
}