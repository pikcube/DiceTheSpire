using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Cards.Token;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Interfaces;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Powers;

[UsedImplicitly]
public class GadgetPower : TheInventorPower, IGadgetParent
{
    static GadgetPower()
    {
        ModHelper.SubscribeForCombatStateHooks("TheInventor.GadgetPower", CombatStateHooks);
    }

    private static GadgetModel[] CombatStateHooks(CombatState state)
    {
        return [.. state.Players.SelectMany(p => p.Creature.Powers.OfType<GadgetPower>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Combat)];
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => MutableDisplayAmount < 1 ? PowerStackType.Single : PowerStackType.Counter;
    public override int DisplayAmount => MutableDisplayAmount;

    private int MutableDisplayAmount
    {
        get;
        set
        {
            int newVal = value < 1 ? 0 : value;
            bool changed = newVal != field;
            if (!changed)
            {
                return;
            }
            field = newVal;
            InvokeDisplayAmountChanged();
        }
    }


    public void SetValue(int display)
    {
        MutableDisplayAmount = display;
    }

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(nameof(GadgetText)), new StringVar(nameof(GadgetName))];

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

    public override LocString Title => new LocString("powers", Id.Entry + ".title").WithDynamicVars(DynamicVars);

    public string GadgetId
    {
        get;
        set
        {
            field = value;
            GadgetText = $"Gadget: {LinkedGadgetModel.Description.GetFormattedText()}";
        }
    } = nameof(DefaultGadget);

    Player IGadgetParent.Owner => Owner.Player ?? throw new InvalidOperationException();
    void IGadgetParent.Flash() => Flash();

    public AbstractModel AsModel() => this;

    public GadgetModel LinkedGadgetModel
    {
        get
        {
            if (field?.GadgetId != GadgetId || field.Parent != this)
            {
                field = ScrapManager.AllGadgets[GadgetId].GetMutable(this);
                field.OnFirstCharge();
            }
            return field;
        }
    }

    public string InitialGadgetId //Used to preload the name before we apply the power
    {
        get;
        set
        {
            field = value;
            GadgetText = $"Gadget: {ScrapManager.AllGadgets[field].Description.GetFormattedText()}";
            GadgetName = ScrapManager.AllGadgets[field].Title.GetFormattedText();
        }
    } = nameof(DefaultGadget);

    public void Update()
    {
        GadgetText = $"Gadget: {LinkedGadgetModel.Description.GetFormattedText()}";
        GadgetName = LinkedGadgetModel.Title.GetFormattedText();
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
        }

        GadgetId = InitialGadgetId;
    }

    public async Task AfterRandomizedAsync()
    {
        Update();
        await GadgetCard1.ShowAsync(LinkedGadgetModel);
    }
}