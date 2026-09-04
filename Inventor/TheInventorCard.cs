using BaseLib.Extensions;
using BaseLib.Utils;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Inventor.Token;
using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor;

[Pool(typeof(TheInventorCardPool))]
public abstract class TheInventorCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    DiceTheSpireCard(cost, type, rarity, target)
{
    public static bool ShowGadgetTips(CardModel card) => EnableGadgetTipsGlobal || EnableTipsOnCards.Contains(card);
    public static List<CardModel> EnableTipsOnCards { get; } = [];
    public static bool EnableGadgetTipsGlobal { get; set; }

    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath(); //Intentionally using base function

    protected virtual IEnumerable<IHoverTip> ExtraInventorHoverTips => [];

    protected sealed override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        ..GetGadgetHoverTip() , 
        ..GetHeldHoverTip(), 
        ..ExtraInventorHoverTips, 
        ..GetScrapHoverTip()
    ];

    private IEnumerable<IHoverTip> GetGadgetHoverTip()
    {
        if (!ShowGadgetTips(this))
        {
            yield break;
        }

        GadgetModel gadgetModel = ScrapManager.AllGadgets[GetScrapId];
        if (gadgetModel is DefaultGadget)
        {
            yield break;
        }

        GadgetCard gadgetCard = GadgetCard1.Create();
        gadgetCard.SetVars(gadgetModel);
        yield return new CardHoverTip(gadgetCard);
    }

    private IEnumerable<IHoverTip> GetHeldHoverTip()
    {
        if (HasTurnEndInHandEffect)
        {
            yield return HoverTipFactory.Static(BetterStaticHoverTips.Held);
        }
    }

    private IEnumerable<IHoverTip> GetScrapHoverTip()
    {
        if (this is not IScrapCard { IsAlwaysOfferedAsScrap: true })
        {
            yield break;
        }

        yield return HoverTipFactory.Static(InventorStaticHoverTips.Scrap);
        yield return HoverTipFactory.Static(InventorStaticHoverTips.Gadget);
    }

    public abstract string GetScrapId { get; }

    public virtual Task OnScrapAsync()
    {
        return Task.CompletedTask;
    }

    public virtual bool ModifyScrap()
    {
        return false;
    }
}