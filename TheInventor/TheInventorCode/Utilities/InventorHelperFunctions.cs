using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Utilities;

public static class InventorHelperFunctions
{
    public static async Task ApplyRandomDebuffAsync(PlayerChoiceContext choiceContext, IRunState runState, Creature target, Creature? applier, CardModel? cardSource, bool silent = false)
    {
        PowerModel power;
        decimal amount;
        switch (runState.Rng.CombatOrbGeneration.NextInt(target.HasPower<FreezePower>() ? 8 : 9))
        {
            case 8:
                power = ModelDb.Power<FreezePower>().StrongMutableClone();
                amount = 1;
                break;
            case 7:
                if (target.IsPlayer)
                {
                    power = ModelDb.Power<EnergyDownNextTurnPower>().StrongMutableClone();
                }
                else
                {
                    power = ModelDb.Power<ShrinkPower>().StrongMutableClone();
                }

                amount = 1;
                break;
            case 6:
                if (target.IsPlayer)
                {
                    //todo Shock Next Turn
                    power = ModelDb.Power<ShockPower>().StrongMutableClone();
                    amount = 2;
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
                if (target.IsPlayer)
                {
                    power = ModelDb.Power<FrailPower>().StrongMutableClone();
                    amount = 1;
                }
                else
                {
                    power = ModelDb.Power<PoisonPower>().StrongMutableClone();
                    amount = 4;
                }
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
    public static async Task AutoPlayFromDrawPileAndBlink(
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
                await card.BlinkAsync(choiceContext);
                continue;
            }

            if (card.Type != CardType.Power)
            {
                card.ShouldBlinkOnNextPlay = true;
            }

            await CardCmd.AutoPlay(choiceContext, card, null);
        }
    }
}