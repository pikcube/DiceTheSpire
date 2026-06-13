using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheInventor.TheInventorCode.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class GadgetCard : CustomCardModel
{

    public GadgetCard() : base(-1, CardType.Status, CardRarity.Token, TargetType.Self)
    {
        TitleLocString.Add(DynamicVars["GadgetTitle"]);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("GadgetTitle", "Gadget"), new StringVar("GadgetDescription", "Does something.")];


    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    public void SetVars(GadgetModel linkedGadgetModel)
    {
        StringVar title = (StringVar)DynamicVars["GadgetTitle"];
        StringVar desc = (StringVar)DynamicVars["GadgetDescription"];
        title.StringValue = linkedGadgetModel.Title.GetFormattedText();
        desc.StringValue = linkedGadgetModel.Description.GetFormattedText();
        TitleLocString.Add(title);
    }

    public void ResetVars()
    {
        StringVar title = (StringVar)DynamicVars["GadgetTitle"];
        StringVar desc = (StringVar)DynamicVars["GadgetDescription"];
        title.StringValue = "Gadget";
        desc.StringValue = "Does something.";
        TitleLocString.Add(title);
    }
}