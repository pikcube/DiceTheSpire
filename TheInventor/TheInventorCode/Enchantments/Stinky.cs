using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheInventor.TheInventorCode.Enchantments;

public class Stinky : CustomEnchantmentModel
{
    protected override string CustomIconPath => $"res://{MainFile.ModId}/images/enchantments/{nameof(Stinky).ToLowerInvariant()}.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(CardKeyword.Eternal)
    ];
    protected override void OnEnchant()
    {
        Card.AddKeyword(CardKeyword.Eternal);
    }
}