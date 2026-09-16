using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Relics;



[UsedImplicitly]
public class AthleticSponsership : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
    {
        if (Owner != player || options.Source == CardCreationSource.Shop || options is null || cardRewards.ToList<CardCreationResult>() is null)
        {
            return false;
        }
        
        List<CardCreationResult> list = cardRewards.ToList<CardCreationResult>();
        if (list.Count == 0)
            return false;
        CardCreationResult cardCreationResult = Owner.RunState.Rng.Niche.NextItem(list);
        if (cardCreationResult == null)
            return false;
        CardModel card = this.Owner.RunState.CloneCard(cardCreationResult.Card);
        CardCmd.Upgrade(card);
        cardCreationResult.ModifyCard(card, (RelicModel)this);
        return true;

    }
    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<CombatSpatula>();
    }
}