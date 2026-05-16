using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace TheThief.TheThiefCode.Relics;

public class StickyFingers : TheThiefRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> options,
        CardCreationOptions creationOptions)
    {
        if (Owner != player || creationOptions.Source != CardCreationSource.Encounter)
        {
            return false;
        }

        IEnumerable<CardModel> cardModels = ModelDb.AllCards
            .Where(c => 
                player.UnlockState.Cards.Contains(c) &&
                c.Pool.DeckEntryCardColor != player.Character.NameColor && 
                c.Rarity == CardRarity.Common && 
                options.TrueForAll(o => o.originalCard.Id != c.Id)
                );

        cardModels = Owner.RunState.Players.Count > 1 
            ? cardModels.Where(c => c.MultiplayerConstraint != CardMultiplayerConstraint.SingleplayerOnly) 
            : cardModels.Where(c => c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly);


        CardModel? card = cardModels.TakeRandom(1, Owner.PlayerRng.Rewards).SingleOrDefault()?.CreateNewInstance(Owner);

        if (card == null)
        {
            return false;
        }

        CardCreationResult cardCreationResult = new(card);
        cardCreationResult.ModifyCard(card, this);
        options.Add(cardCreationResult);
        return true;
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<StickyHand>();
    }
}