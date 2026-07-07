using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Patches;

namespace TheInventor.TheInventorCode.Gadgets;

public class SharedInterest() : GadgetModel(nameof(SharedInterest))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Run;

    public override bool IsAllowedAsTempGadget => false;

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Parent?.Owner)
        {
            return false;
        }

        if (player.RunState.Players.Count == 1)
        {
            BreakMe();
        }

        foreach (Player p in player.RunState.Players.Where(p => p != player))
        {
            List<CardModel> deck = //Get their deck
            [
                .. p.Deck.Cards.Where(c => c.Rarity is CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare)
            ];

            if (deck.Count < 6)
            {
                //If your ally doesn't have at least 6 common, uncommon, or rares, we populate the rest with basics
                CardModel[] add = 
                [
                    .. p.Deck.Cards.Where(c => c.Rarity == CardRarity.Basic)
                        .TakeRandom(6 - deck.Count, player.PlayerRng.Rewards)
                ];
                deck.AddRange(add);
            }
            if (deck.Count < 6)
            {
                //If your ally has less than 6 cards in their deck, we'll pull from the unlocked cards in the pool
                CardModel[] add =
                [
                    .. p.Character.CardPool
                        .GetUnlockedCards(player.UnlockState, CardMultiplayerConstraint.MultiplayerOnly)
                        .TakeRandom(6 - deck.Count, player.PlayerRng.Rewards)
                ];
                deck.AddRange(add);
            }
            if (deck.Count < 6)
            {
                //If there still aren't 6 cards, I guess we ignore the unlock state and pull from the common pool
                CardModel[] add =
                [
                    ..p.Character.CardPool.AllCards.Where(c =>
                            c.Rarity is CardRarity.Common &&
                            c.MultiplayerConstraint != CardMultiplayerConstraint.SingleplayerOnly)
                        .TakeRandom(6 - deck.Count, player.PlayerRng.Rewards)
                ];
                deck.AddRange(add);
            }
            while (deck.Count < 6)
            {
                //If we somehow still don't have enough cards, just give them claws I guess. /shrug
                deck.Add(Claw.Create());
            }

            //Now we can actually generate the version that can go into your deck
            for (int n = 0; n < deck.Count; ++n)
            {
                CardModel card = deck[n];
                card = card.StrongMutableClone();
                card.Owner = null!;
                card.Owner = player;
                deck[n] = player.RunState.CloneCard(card);
            }

            player.PlayerRng.Rewards.Shuffle(deck);

            List<CardModel> backupCards = deck[3..];

            CardReward cardReward = new(deck[..3], CardCreationSource.Encounter, player, new CardCreationOptions([], CardCreationSource.Encounter, CardRarityOddsType.Uniform));
            cardReward.Populate();
            rewards.Add(cardReward);
            CardRewardRerollPatch.HijackReroll(cardReward, backupCards);
        }

        BreakMe();
        return true;
    }
}