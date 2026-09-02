using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Interfaces;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpire.DiceTheSpireCode.Inventor.Token;
using DiceTheSpire.DiceTheSpireCode.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Powers;

[UsedImplicitly]
public class TemporaryGadgetPower : TheInventorPower, IGadgetParent
{
    static TemporaryGadgetPower()
    {
        ModHelper.SubscribeForCombatStateHooks("TheInventor.TemporaryGadgetPower", CombatStateHooks);
    }

    private static GadgetModel[] CombatStateHooks(CombatState state)
    {
        return [.. state.Players.SelectMany(p => p.Creature.Powers.OfType<TemporaryGadgetPower>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Combat)];
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
                field.OnFirstCharge();
            }
            return field;
        }
    }

    public void Update()
    {
        GadgetText = $"{LinkedGadgetModel.GadgetText}";
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => GetStackType();
    public override int DisplayAmount => DispAmount;

    private int DispAmount
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

    private PowerStackType GetStackType() => DispAmount < 1 ? PowerStackType.Single : PowerStackType.Counter;

    public void SetValue(int display)
    {
        DispAmount = display;
    }
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
        }
    }

    public async Task SetThisAsync(string id)
    {
        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
            return;
        }

        GadgetId = id;
        await AfterRandomizedAsync();
    }

    public async Task RandomizeThisAsync()
    {
        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
            return;
        }

        GadgetId = ScrapManager.GetRandomCombatGadgetId(Owner.Player.RunState.Rng.CombatOrbGeneration);
        await AfterRandomizedAsync();
    }

    public async Task AfterRandomizedAsync()
    {
        await GadgetCard1.ShowAsync(LinkedGadgetModel);
    }
}
