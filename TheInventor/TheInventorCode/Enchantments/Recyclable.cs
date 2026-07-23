using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Enchantments;

public class Recyclable : CustomEnchantmentModel, IScrapCard
{
    protected override string CustomIconPath => $"res://{MainFile.ModId}/images/enchantments/{nameof(Recyclable).ToLowerInvariant()}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(InventorStaticHoverTips.Scrap),
        HoverTipFactory.Static(InventorStaticHoverTips.Gadget)
    ];

    public override bool CanEnchant(CardModel card)
    {
        return ScrapManager.CanScrapCard(card);
    }

    public bool IsAlwaysOfferedAsScrap => true;

    public override bool HasExtraCardText => true;
}