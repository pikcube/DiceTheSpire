using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using static BaseLib.Utils.BetaMainCompatibility;

namespace TheThief.TheThiefCode.Relics;

public class StickyFingers : TheThiefRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> options,
        CardCreationOptions creationOptions)
    {
        if (this.Owner != player || creationOptions.Source != CardCreationSource.Encounter)
        {
            return false;
        }

        IEnumerable<CardModel> cardModels = creationOptions.GetPossibleCards(player).Where<CardModel>((Func<CardModel, bool>)(c => c.Pool.DeckEntryCardColor != player.Character.NameColor)).Where<CardModel>((Func<CardModel, bool>)(c => c.Rarity == CardRarity.Common && options.TrueForAll((Predicate<CardCreationResult>)(o => o.originalCard.Id != c.Id))));
        if (!cardModels.Any<CardModel>())
        {
            cardModels = creationOptions.GetPossibleCards(player).Where<CardModel>((Func<CardModel, bool>)(c => c.Pool.DeckEntryCardColor != player.Character.NameColor)).Where<CardModel>((Func<CardModel, bool>)(c => c.Rarity == CardRarity.Common));
        }

        if (!cardModels.Any<CardModel>())
        {
            return false;
        }

        CardModel card = CardFactory.CreateForReward(this.Owner, 1, new CardCreationOptions(cardModels, CardCreationSource.Other, creationOptions.RarityOdds).WithFlags(CardCreationFlags.NoModifyHooks | CardCreationFlags.NoCardPoolModifications)).FirstOrDefault<CardCreationResult>()?.Card;
        if (card != null)
        {
            CardCreationResult cardCreationResult = new CardCreationResult(card);
            cardCreationResult.ModifyCard(card, (RelicModel)this);
            options.Add(cardCreationResult);
        }
        return card != null;
    }
    /*
    public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
    {
        if (this.Owner != player || options.Flags.HasFlag((Enum) CardCreationFlags.NoCardPoolModifications) || !options.Flags.HasFlag((Enum) CardCreationFlags.IsCardReward) || options.CustomCardPool != null || options.CardPools.All<CardPoolModel>((Func<CardPoolModel, bool>)(p => p.IsColorless)))
            return options;
        IEnumerable<CardPoolModel> pools = player.UnlockState.CharacterCardPools.Union<CardPoolModel>((IEnumerable<CardPoolModel>)options.CardPools);
        return options.WithCardPools(pools, options.CardPoolFilter);
    }
    */
    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<StickyHand>();
    }
}