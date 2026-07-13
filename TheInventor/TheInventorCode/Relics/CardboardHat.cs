using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Enchantments;

namespace TheInventor.TheInventorCode.Relics;

public class CardboardHat : TheInventorRelic
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..HoverTipFactory.FromEnchantment<Recyclable>()];

    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new(CardSelectorPrefs.EnchantSelectionPrompt, 3);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromDeckForEnchantment(Owner, ModelDb.Enchantment<Recyclable>(), 1, prefs);
        foreach (CardModel card in cards)
        {
            CardCmd.Enchant<Recyclable>(card, 1);
        }
    }
}