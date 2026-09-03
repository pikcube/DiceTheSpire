using BaseLib.Abstracts;
using DiceTheSpire.Common.Interfaces;
using DiceTheSpire.Common.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Enchantments;

public class Recyclable : CustomEnchantmentModel, IScrapCard
{
    protected override string CustomIconPath => $"{DiceTheSpire.MainFile.ResPath}/images/enchantments/{nameof(Recyclable).ToLowerInvariant()}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(InventorStaticHoverTips.Scrap),
        HoverTipFactory.Static(InventorStaticHoverTips.Gadget)
    ];

    public override bool CanEnchant(CardModel card)
    {
        if (card.Type is CardType.Status or CardType.Quest)
        {
            return false;
        }

        if (card.Enchantment is not null)
        {
            return false;
        }

        return !ScrapManager.IsAlwaysOfferedAsScrap(card) && ScrapManager.CanScrapCard(card);
    }

    public bool IsAlwaysOfferedAsScrap => true;

    public override bool HasExtraCardText => true;
}