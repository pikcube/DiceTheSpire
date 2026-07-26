using System.Data;
using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Token;

public abstract class GadgetCard() : CustomCardModel(-1, CardType.Power, CardRarity.Token, TargetType.Self, false)
{
    public override int MaxUpgradeLevel => 0;

    public override string Title => ((StringVar)DynamicVars["GadgetTitle"]).StringValue;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("GadgetTitle", "Gadget"), new StringVar("GadgetDescription", "Does something.")];

    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => "gadget_card.png".CardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => "gadget_card.png".CardImagePath();
    public override string BetaPortraitPath => "beta/gadget_card.png".CardImagePath();

    public override bool CanBeGeneratedByModifiers => false;
    public override bool CanBeGeneratedInCombat => false;

    public void SetVars(GadgetModel linkedGadgetModel, bool isForPopUp = false)
    {
        StringVar title = (StringVar)DynamicVars["GadgetTitle"];
        StringVar desc = (StringVar)DynamicVars["GadgetDescription"];
        title.StringValue = linkedGadgetModel.Title.GetFormattedText();
        desc.StringValue = isForPopUp 
            ? linkedGadgetModel.Description.GetFormattedText() 
            : $"{linkedGadgetModel.Description.GetFormattedText()}\n{linkedGadgetModel.Duration.GetFormattedText()}";
    }

    public void ResetVars()
    {
        StringVar title = (StringVar)DynamicVars["GadgetTitle"];
        StringVar desc = (StringVar)DynamicVars["GadgetDescription"];
        title.StringValue = "Gadget";
        desc.StringValue = "Does something.";
        TitleLocString.Add(title);
    }

    public async Task ShowAndDestoryCardAsync(float delayTimeBasedOnIndex)
    {
        Control cardPreviewContainer = NRun.Instance?.GlobalUi.CardPreviewContainer ?? throw new NoNullAllowedException();
        NCard nCard = NCard.Create(this) ?? throw new NoNullAllowedException();
        cardPreviewContainer.AddChildSafely(nCard);
        nCard.UpdateVisuals(PileType.Exhaust, CardPreviewMode.Normal);
        Tween tween = nCard.CreateTween();
        tween.TweenProperty(nCard, (NodePath)"scale", Vector2.One, 0.25)
            .From(Vector2.Zero)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
        tween.TweenInterval(delayTimeBasedOnIndex);
        tween.TweenCallback(Callable.From((Action)(() => { NRun.Instance.GlobalUi.AddChildSafely(NCardExhaustVfx.Create(nCard)!); })));
        tween.TweenProperty(nCard, (NodePath)"modulate", StsColors.exhaustGray,
            SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.2 : 0.3);
        tween.TweenCallback(Callable.From(nCard.QueueFree));
        tween.TweenCallback(Callable.From(ResetVars));

        await tween.AwaitFinished(nCard);
    }
}

[Pool(typeof(TokenCardPool))]
public class GadgetCard1 : GadgetCard
{ 
    public static async Task ShowAsync(GadgetModel linkedGadgetModel)
    {
        if (linkedGadgetModel.Parent is null || LocalContext.IsMe(linkedGadgetModel.Parent.Owner))
        {
            GadgetCard gadgetCard = GadgetCard1.Create();
            gadgetCard.SetVars(linkedGadgetModel, true);
            await gadgetCard.ShowAndDestoryCardAsync(0.5f);
        }
    }
}

[Pool(typeof(TokenCardPool))]
public class GadgetCard2 : GadgetCard
{ 
    public static async Task ShowAsync(GadgetModel linkedGadgetModel)
    {
        if (linkedGadgetModel.Parent is null || LocalContext.IsMe(linkedGadgetModel.Parent.Owner))
        {
            GadgetCard gadgetCard = GadgetCard2.Create();
            gadgetCard.SetVars(linkedGadgetModel, true);
            await gadgetCard.ShowAndDestoryCardAsync(0.5f);
        }
    }
}

[Pool(typeof(TokenCardPool))]
public class GadgetCard3 : GadgetCard
{ 
    public static async Task ShowAsync(GadgetModel linkedGadgetModel)
    {
        if (linkedGadgetModel.Parent is null || LocalContext.IsMe(linkedGadgetModel.Parent.Owner))
        {
            GadgetCard gadgetCard = GadgetCard3.Create();
            gadgetCard.SetVars(linkedGadgetModel, true);
            await gadgetCard.ShowAndDestoryCardAsync(0.5f);
        }
    }
}