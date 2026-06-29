using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Enchantments;
using TheInventor.TheInventorCode.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Keywords;

namespace TheInventor.TheInventorCode.Cards.Ancient;

public class RottenEgg() : TheInventorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.Self), ITomeCard
{
    public override string GetScrapId => nameof(StinkyGadget);
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, ScrapKeyword.Scrap];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [.. HoverTipFactory.FromEnchantment<Stinky>()];
    public override int MaxUpgradeLevel => 0;

    public override async Task OnScrapAsync()
    {
        CardSelectorPrefs prefs = new(CardSelectorPrefs.EnchantSelectionPrompt, 3);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromDeckForEnchantment(Owner, ModelDb.Enchantment<Stinky>(), 1, prefs);
        foreach (CardModel card in cards)
        {
            CardCmd.Enchant<Stinky>(card, 1);
        }
    }

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != Owner)
        {
            return false;
        }

        options.Add(new RottenHatchRestSiteOption(player));
        return true;
    }
}

public class RottenHatchRestSiteOption(Player player) : CustomRestSiteOption(player)
{
    public override async Task<bool> OnSelect()
    {
        RottenEgg? egg = Owner.Deck.Cards.OfType<RottenEgg>().FirstOrDefault();
        if (egg is not null)
        {
            Scorpion scorp = Scorpion.Create(Owner);
            CardCmd.Upgrade(scorp);
            await CardCmd.Transform(egg, scorp);
        }

        List<RottenEgg> eggs = [..Owner.Deck.Cards.OfType<RottenEgg>()];
        foreach (RottenEgg e in eggs)
        {
            await CardPileCmd.RemoveFromDeck(e, false);
        }

        return true;
    }

    public override string OptionId => "THEINVENTOR-HATCH";

    public override string CustomIconPath => "rest_site/hatch.png".ImagePath();
}