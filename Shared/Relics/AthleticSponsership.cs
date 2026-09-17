using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class AthleticSponsership : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    //private int procuredSponsershipFunds { get; set; } = 0;
    public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
    {
        if (Owner != player || options.Source == CardCreationSource.Shop)
        {
            return false;
        }

        if (cardRewards.Count == 0)
        {
            return false;
        }

        if (player.RunState.BaseRoom?.RoomType != RoomType.Elite)
        {
            return false;
        }

        if (options.Flags.HasFlag(CardCreationFlags.NoHookUpgrades))
        {
            return false;
        }

        UpgradeValidCards(cardRewards, _ => true);

        return true;

        //CardCreationResult ?cardCreationResult = Owner.RunState.Rng.Niche.NextItem(list);
        //if (cardCreationResult == null)
        //    return false;
        //CardModel card = this.Owner.RunState.CloneCard(cardCreationResult.Card);
        //if(procuredSponsershipFunds <= 2)
        //{
        //    procuredSponsershipFunds++;
        //    return false;
        //}
        //else
        //{
        //    CardCmd.Upgrade(card);
        //    cardCreationResult.ModifyCard(card, (RelicModel)this);
        //    if (player.RunState.CurrentActIndex == 0)
        //    {
        //        procuredSponsershipFunds -= 2;
        //    }
        //    else if (player.RunState.CurrentActIndex == 1)
        //    {
        //        procuredSponsershipFunds -= 1;
        //    }
        //    else
        //    {
        //        procuredSponsershipFunds = 3;
        //    }
        //}

    }

    //public override Task AfterActEntered()
    //{
    //    procuredSponsershipFunds++;
    //    return base.AfterActEntered();
    //}
    private static void UpgradeValidCards(IEnumerable<CardCreationResult> cards, Predicate<CardModel> filter)
    {
        foreach (CardCreationResult cardCreationResult in cards.Where(c => c.Card.IsUpgradable && filter(c.Card)))
        {
            CardModel card = cardCreationResult.Card.Owner.RunState.CloneCard(cardCreationResult.Card);
            CardCmd.Upgrade(card);
            cardCreationResult.ModifyCard(card);
        }
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<CombatSpatula>();
    }
}