using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Relics;


public class SpareSpectacles : TheInventorRelic, IModifyScrapPriorityListener
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(BetterStaticHoverTips.Scrap), HoverTipFactory.Static(BetterStaticHoverTips.Gadget)
    ];

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