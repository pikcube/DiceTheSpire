using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Relics;

public class StickyFingers : TheThiefRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> options,
        CardCreationOptions creationOptions)
    {
        if (Owner != player || creationOptions.Source == CardCreationSource.Shop)
        {
            return false;
        }

        IEnumerable<CardCreationResult>? card = ThiefHelperFunctions.GetCardsOfClassForReward(player,
            [.. ThiefHelperFunctions.BaseCharacterCardPools, .. ThiefHelperFunctions.DiceyNonThiefCardPools], 1,
            CardRarityOddsType.Uniform, c => c.Rarity == CardRarity.Common);
        if (card is null)
        {
            return false;
        }

        options.Add(card.First());
        return true;
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<StickyHand>();
    }
}