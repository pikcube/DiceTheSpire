using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Interfaces;
using TheInventor.TheInventorCode.Powers;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Gadgets;

public abstract class GadgetModel : AbstractModel, ICustomModel
{ 
    protected GadgetModel(string gadgetId)
    {
        GadgetId = gadgetId;
        Gadget.AllGadgets[GadgetId] = this;
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

    public LocString Title
    {
        get
        {
            field ??= new LocString("gadgets", Id.Entry + ".title").WithDynamicVars(DynamicVars);
            return field;
        }
    }
    public LocString Description
    {
        get
        {
            field ??= new LocString("gadgets", Id.Entry + ".description").WithDynamicVars(DynamicVars);
            return field;
        }
    }

    public virtual bool IsAllowedAsTempGadget => true;
    public abstract CustomSingletonModel.HookType HookType { get; }

    public GadgetModel GetMutable(IGadgetParent gadget)
    {
        GadgetModel newGadgetModel = (GadgetModel)MutableClone();
        newGadgetModel.Parent = gadget;
        return newGadgetModel;
    }

    public virtual Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player) => Task.CompletedTask;

    protected static int GetPower(Player player)
    {
        decimal power = player.RunState
            .IterateHookListeners(player.Creature.CombatState)
            .OfType<IGadgetPowerListener>()
            .Aggregate<IGadgetPowerListener, decimal>(1, (current, listener) => current * listener.ModifyGadgetPowerMultiplicative(player));

        return (int)Math.Round(power);
    }

    public void BreakMe()
    {
        AssertMutable();
        if (IsAllowedAsTempGadget)
        {
            throw new InvalidProgramException("Breakable Gadgets cannot be temporary");
        }

        Parent?.GadgetId = nameof(BrokenGadget);
    }
}