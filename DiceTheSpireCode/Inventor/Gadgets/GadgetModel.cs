using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Common.Interfaces;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

public abstract class GadgetModel : AbstractModel, ICustomModel
{ 
    protected GadgetModel(string gadgetId)
    {
        GadgetId = gadgetId;
        ScrapManager.AllGadgets[GadgetId] = this;
    }

    public IGadgetParent? Parent { get; set; }
    public override bool ShouldReceiveCombatHooks => true;

    protected virtual IEnumerable<DynamicVar> CanonicalVars => [];

    public DynamicVarSet DynamicVars
    {
        get
        {
            field ??= new DynamicVarSet(CanonicalVars).WithOwnerInitialized(this);
            return field;
        }
    }

    public string GadgetId { get; }
    public string GadgetText => string.Join(": ", GadgetLocStrings);
    public virtual IEnumerable<string> GadgetLocStrings => [Title.GetFormattedText(), $"{Description.GetFormattedText()}"];

    public virtual decimal PowerBase => 1;
    public decimal PowerLevel => Parent?.Owner is null ? 1 : GetPower(Parent.Owner);
    public int Power => (int)Math.Round(PowerBase * PowerLevel);

    public LocString Title
    {
        get
        {
            LocString title = new LocString("gadgets", Id.Entry + ".title").WithDynamicVars(DynamicVars);
            title.Add(nameof(Power), Power);
            return title;
        }
    }
    public LocString Description
    {
        get
        {
            LocString description = new LocString("gadgets", Id.Entry + ".description").WithDynamicVars(DynamicVars);
            description.Add(nameof(Power), Power);
            return description;
        }
    }

    public virtual bool IsAllowedAsTempGadget => true;
    public abstract CustomSingletonModel.HookType HookType { get; }

    public LocString Duration
    {
        get
        {
            string key = LocString.Exists("gadgets", Id.Entry + ".duration") ? Id.Entry + ".duration" : $"{MainFile.ModPrefix}-DEFAULT.duration";
            LocString description = new LocString("gadgets", key).WithDynamicVars(DynamicVars);
            description.Add(nameof(Power), Power);
            return description;
        }
    }

    public GadgetModel GetMutable(IGadgetParent gadget)
    {
        GadgetModel newGadgetModel = (GadgetModel)MutableClone();
        newGadgetModel.Parent = gadget;
        return newGadgetModel;
    }

    public virtual Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player) => Task.CompletedTask;

    protected static decimal GetPower(Player player)
    {
        decimal power = player.RunState
            .IterateHookListeners(player.Creature.CombatState)
            .OfType<IGadgetPowerListener>()
            .Aggregate<IGadgetPowerListener, decimal>(1, (current, listener) => current * listener.ModifyGadgetPowerMultiplicative(player));

        return power;
    }

    protected void BreakMe()
    {
        AssertMutable();
        if (IsAllowedAsTempGadget)
        {
            throw new InvalidProgramException("Breakable Gadgets cannot be temporary");
        }

        if (Parent is TempParent tp)
        {
            ScrapManager.GadgetId.Set(tp.Owner, nameof(BrokenGadget));
            tp.GadgetId = nameof(BrokenGadget);
            tp.LinkedGadgetModel = ScrapManager.AllGadgets[nameof(BrokenGadget)].GetMutable(tp);
        }
        else
        {
            Parent?.GadgetId = nameof(BrokenGadget);
        }
    }

    public virtual void OnFirstCharge()
    {
    }

    public virtual Task OnPickupAsync() => Task.CompletedTask;
}