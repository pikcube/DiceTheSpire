using DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Relics;


public class SpareSpectacles : TheInventorRelic, IModifyScrapPriorityListener
{
    public override RelicRarity Rarity => RelicRarity.Common;
    public void ModifyPriority(Player player, ref List<CardModel> scrapCards, ref List<CardModel> otherCards)
    {
        if (player != Owner)
        {
            return;
        }

        CardModel? card = otherCards
            .Where(c => !c.IsUpgraded)
            .TakeRandom(1, player.PlayerRng.Rewards)
            .SingleOrDefault();

        if (card is null)
        {
            return;
        }

        otherCards.Remove(card);
        scrapCards.Add(card);
    }
}