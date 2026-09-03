using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Enchantments;

public class Stinky : CustomEnchantmentModel
{
    protected override string CustomIconPath => $"{MainFile.ResPath}/images/enchantments/{nameof(Stinky).ToLowerInvariant()}.png";

    public override bool CanEnchant(CardModel card)
    {
        if (card.Type is CardType.Status or CardType.Curse or CardType.Quest)
        {
            return false;
        }

        if (card.Enchantment is not null)
        {
            return false;
        }

        return card.IsRemovable;
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(CardKeyword.Eternal)
    ];
    protected override void OnEnchant()
    {
        Card.AddKeyword(CardKeyword.Eternal);
    }
}