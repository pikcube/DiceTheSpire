using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Relics;

public class StickyHand : TheThiefRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> options,
        CardCreationOptions creationOptions)
    {
        if (Owner != player || creationOptions.Source == CardCreationSource.Shop)
        {
            return false;
        }

        IEnumerable<CardCreationResult> card = ThiefHelperFunctions.GetCardsOfClassForReward(player,
            ThiefHelperFunctions.NonThiefCharacterPools, 1,
            creationOptions.RarityOdds);

        options.AddRange(card);
        return true;
    }
}