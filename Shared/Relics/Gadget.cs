using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Inventor.Powers;
using DiceTheSpire.Inventor.Token;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Relics;

[Pool(typeof(SharedRelicPool))]
public class Gadget : DiceTheSpireRelic, IGadgetParent
{
    static Gadget()
    {
        ModHelper.SubscribeForRunStateHooks("TheInventor.GadgetRelic", RunStateHooks);
    }

    public override bool HasUponPickupEffect => true;

    private static GadgetModel[] RunStateHooks(RunState state)
    {
        return [.. state.Players
            .Where(p => !p.HasPower<GadgetPower>())
            .SelectMany(p => p.Relics.OfType<Gadget>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Combat)];
    }

    public override RelicRarity Rarity => RelicRarity.Starter;


    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new StringVar(nameof(GadgetText), ScrapManager.AllGadgets[nameof(HeatRay)].Description.GetFormattedText()),
        new StringVar(nameof(GadgetName), ScrapManager.AllGadgets[nameof(HeatRay)].Title.GetFormattedText())
    ];

    public string GadgetName
    {
        get => ((StringVar)DynamicVars[nameof(GadgetName)]).StringValue;
        set => ((StringVar)DynamicVars[nameof(GadgetName)]).StringValue = value;
    }

    private string GadgetText
    {
        get => ((StringVar)DynamicVars[nameof(GadgetText)]).StringValue;
        set => ((StringVar)DynamicVars[nameof(GadgetText)]).StringValue = value;
    }

    public override LocString Title => base.Title.WithDynamicVars(DynamicVars);

    public string GadgetId
    {
        get;
        set
        {
            field = value;
            Update();
        }
    } = nameof(DefaultGadget);

    public GadgetModel LinkedGadgetModel
    {
        get
        {
            if (field?.GadgetId == GadgetId && field.Parent == this)
            {
                return field;
            }

            field = ScrapManager.AllGadgets[GadgetId].GetMutable(this);
            field.OnFirstCharge();
            return field;
        }
    }
    public async Task UpdateAndPreviewAsync()
    {
        Update();
        await GadgetCard1.ShowAsync(LinkedGadgetModel);
    }

    public void Update()
    {
        GadgetName = LinkedGadgetModel.Title.GetFormattedText();
        GadgetText = LinkedGadgetModel.Description.GetFormattedText();
    }

    public void SetValue(int display)
    {
        
    }
}