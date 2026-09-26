using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Patches;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Utility;

public static class InventorHelperFunctions
{
    public static async Task ApplyRandomDebuffAsync(PlayerChoiceContext choiceContext, IRunState runState, Creature target, Creature? applier, CardModel? cardSource, bool silent = false)
    {
        PowerModel power;
        decimal amount;
        switch (runState.Rng.CombatOrbGeneration.NextInt(target.HasPower<FreezePower>() ? 8 : 9))
        {
            case 8:
                power = ModelDb.Power<FreezePower>().StrongMutableClone(); //This is the only debuff in this table that doesn't stack
                amount = 1;                                                //So we exclude it if the target is already frozen
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

        List<CardModel> cards = new(count);
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

            cards.Add(card);
            await CardPileCmd.Add(card, PileType.Play);
        }
        foreach (CardModel card in cards.TakeWhile(card => !card.Owner.Creature.IsDead))
        {
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
    private static bool IsDebuffAtAmount(PowerModel power, decimal amount)
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
}