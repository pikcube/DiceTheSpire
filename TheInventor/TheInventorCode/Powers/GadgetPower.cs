using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
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

    public override PowerStackType StackType => GetStackType();

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    private PowerStackType GetStackType()
    {
        return PowerStackType.Single;
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(nameof(GadgetText))];

    private string GadgetText
    {
        get => ((StringVar)DynamicVars[nameof(GadgetText)]).StringValue;
        set => ((StringVar)DynamicVars[nameof(GadgetText)]).StringValue = value;
    }

    public string GadgetId
    {
        get;
        set
        {
            field = value;
            GadgetText = $"{LinkedGadgetModel.GadgetText}";
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
            }
            return field;
        }
    }

    public string InitialGadgetId
    {
        get;
        set
        {
            field = value;
            GadgetText = $"{ScrapManager.AllGadgets[field].GadgetText}";
        }
    } = nameof(DefaultGadget);

    public void Update()
    {
        GadgetText = $"{LinkedGadgetModel.GadgetText}";
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
        await GadgetCard.ShowAsync(LinkedGadgetModel);
    }
}