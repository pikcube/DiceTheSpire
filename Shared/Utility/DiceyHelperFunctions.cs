using System.Data;
using System.Reflection;
using DiceTheSpire.Inventor;
using DiceTheSpire.Inventor.Powers;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Patches;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Thief;
using DiceTheSpire.Warrior;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Utility;

public static class DiceyHelperFunctions
{
    public static async Task ApplyRandomDebuffAsync(PlayerChoiceContext choiceContext, IRunState runState, Creature target, Creature? applier, CardModel? cardSource, bool silent = false)
    {
        PowerModel power;
        decimal amount;

        switch (GetNextRngForRandomDebuffs(runState, target, 9))
        {
            case 8:
                power = ModelDb.Power<FreezePower>().StrongMutableClone(); //This is the only debuff in this table that doesn't stack
                amount = 1;                                                //So we exclude it if the target is already frozen or if the target is a player
                break;
            case 7:
                if (target.IsPlayer)
                {
                    power = ModelDb.Power<EnergyDownNextTurnPower>().StrongMutableClone();
                    amount = 1;
                }
                else
                {
                    power = ModelDb.Power<PoisonPower>().StrongMutableClone();
                    amount = 4;
                }
                break;
            case 6:
                if (target.IsPlayer)
                {
                    power = ModelDb.Power<NoDrawPower>().StrongMutableClone();
                    amount = 1;
                }
                else
                {
                    power = ModelDb.Power<DarkShacklesPower>().StrongMutableClone();
                    amount = 4;
                }
                break;
            case 5:
                power = ModelDb.Power<DoomPower>().StrongMutableClone();
                amount = 5;
                break;
            case 4:
                power = ModelDb.Power<ExhaustionPower>().StrongMutableClone();
                amount = 1;
                break;
            case 3:
                power = ModelDb.Power<ShrinkPower>().StrongMutableClone();
                amount = 1;
                break;
            case 2:
                power = ModelDb.Power<DebilitatePower>().StrongMutableClone();
                amount = 1;
                break;
            case 1:
                power = ModelDb.Power<WeakPower>().StrongMutableClone();
                amount = 1;
                break;
            default:
                power = ModelDb.Power<VulnerablePower>().StrongMutableClone();
                amount = 1;
                break;
        }

        await PowerCmd.Apply(choiceContext, power, target, amount, applier, cardSource, silent);
    }

    private static int GetNextRngForRandomDebuffs(IRunState runState, Creature target, int max)
    {
        if (target.HasPower<FreezePower>() || target.IsPlayer)
        {
            --max; //Freeze doesn't stack and realistically shouldn't be applied to the player, so we remove that index
            //todo: Write a struct of some sort for this so we don't need to keep track of IDs like we're writing Assembly
        }
        return runState.Rng.CombatOrbGeneration.NextInt(max); //todo: swap this for a less stupid rng function
    }

    /// <summary>
    /// Play cards directly from the draw pile.
    /// If the draw pile becomes empty before the specified number of cards are played, the discard pile will
    /// automatically be shuffled into it.
    /// </summary>
    /// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
    /// <param name="player">Player whose draw pile we should play from.</param>
    /// <param name="count">Number of cards to play.</param>
    /// <param name="position">Position to play the cards from.</param>
    public static async Task AutoPlayFromDrawPileAndShock(
      PlayerChoiceContext choiceContext,
      Player player,
      int count,
      CardPilePosition position)
    {
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }

        CardPile drawPile = PileType.Draw.GetPile(player);
        for (int i = 0; i < count; ++i)
        {
            await CardPileCmd.ShuffleIfNecessary(choiceContext, player);
            if (drawPile.Cards.Count == 0)
            {
                break;
            }
            CardModel? card = position switch
            {
                CardPilePosition.Bottom => drawPile.Cards[^1],
                CardPilePosition.Top => drawPile.Cards[0],
                _ => player.RunState.Rng.CombatCardSelection.NextItem(drawPile.Cards)
            };
            if (card == null)
            {
                break;
            }

            if (card.Keywords.Contains(CardKeyword.Unplayable))
            {
                await card.ShockAsync(choiceContext);
                continue;
            }

            if (card.Type != CardType.Power)
            {
                card.ShouldShockOnNextPlay = true;
            }

            await CardCmd.AutoPlay(choiceContext, card, null);
        }
    }

    public static async Task ShockRandomAsync(PlayerChoiceContext choiceContext, Player player, Rng rng, int count)
    {
        CardModel[] cardsToShock = [.. PileType.Hand.GetPile(player).Cards.TakeRandom(count, rng)];
        foreach (CardModel card in cardsToShock)
        {
            await card.ShockAsync(choiceContext);
        }
    }

    /// <summary>
    /// Check if a power application is a self debuff.
    /// </summary>
    /// <param name="owner">The "self" in self debuff.</param>
    /// <param name="power">The power we are applying (can be the canonical instance).</param>
    /// <param name="target">The target of teh debuff.</param>
    /// <param name="amount">The amount we are applying.</param>
    /// <param name="applier">The creature applying the debuff (if any).</param>
    /// <returns>True if this passes all the checks for being a self debuff.</returns>
    public static bool IsSelfDebuff(Creature owner, PowerModel power, Creature target, decimal amount, Creature? applier)
    {
        return target == owner && 
               applier == owner && 
               IsDebuffAtAmount(power, amount) &&
               !IsBannedDebuff(power);
    }

    /// <summary>
    /// Helper function to check if a power is a debuff at the set amount.
    /// We can't just use the built in one due to weird edge cases surrounding negative amounts.
    /// </summary>
    /// <param name="power">The power we are applying.</param>
    /// <param name="amount">The amount we are applying.</param>
    /// <returns>True if we should consider this a debuff at this amount.</returns>
    public static bool IsDebuffAtAmount(PowerModel power, decimal amount)
    {
        if (amount == 0)
        {
            return false; //Doing nothing is not a debuff
        }

        switch (power) //Technically there's only one branch here, but something tells me we might need to add more special cases later
        {
            case ThornsPower:
                return amount < 0; //Thorns is typically never considered a debuff,but we need to override 
                                   //the default logic for Rosewood Spear to work correctly with the archetype
            default:
                if (amount > 0)
                {
                    return power.GetTypeForAmount(amount) is PowerType.Debuff; //If amount is positive, check that it is a debuff at this amount
                }

                return power.GetTypeForAmount(amount) is PowerType.Debuff && //If the amount is negative, also check if it's still a debuff when positive
                       power.GetTypeForAmount(-amount) is not PowerType.Debuff; //Otherwise Safety Goggles will stop a debuff from wearing off
        }
    }

    /// <summary>
    /// A helper function to deal with Knowledge Demon's debuffs setting the player as the applier (which is unintuitive).
    /// </summary>
    /// <param name="power">The power to check against</param>
    /// <returns>True is this should not be considered a self debuff.</returns>
    private static bool IsBannedDebuff(PowerModel power)
    {
        return power switch
        {
            MindRotPower => MindRotPatch.IsKnowledgeDemonDoingStuff,
            SlothPower => SlothPatch.IsKnowledgeDemonDoingStuff,
            WasteAwayPower => WasteAwayPatch.IsKnowledgeDemonDoingStuff,
            DisintegrationPower => DisintegrationPatch.IsKnowledgeDemonDoingStuff,
            _ => false
        };
    }

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

    public static Type AsyncTypeOf<T>()
    {
        return AsyncTypeOf(typeof(T));
    }

    public static Type AsyncTypeOf(Type t)
    {
        Type? patchTypeTarget = null;
        string? methodName = null;
        MethodType? methodType = null;

        foreach (HarmonyPatch attribute in t.GetCustomAttributes<HarmonyPatch>())
        {
            patchTypeTarget ??= attribute.info.declaringType;
            methodName ??= attribute.info.methodName;
            methodType ??= attribute.info.methodType;
        }

        if (patchTypeTarget is null || methodName is null || methodType is not MethodType.Async)
        {
            throw new NoNullAllowedException();
        }

        MethodInfo asyncMethodBeingPatched = AccessTools.DeclaredMethod(patchTypeTarget, methodName);
        return AccessTools.AsyncMoveNext(asyncMethodBeingPatched).DeclaringType ?? throw new NoNullAllowedException();
    }
}