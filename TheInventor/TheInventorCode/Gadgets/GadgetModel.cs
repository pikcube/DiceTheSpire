using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Interfaces;
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
    public virtual IEnumerable<string> GadgetLocStrings => [Title.GetRawText(), $"{Description.GetRawText()}"];

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

    public GadgetModel GetMutable(IGadgetParent gadget)
    {
        GadgetModel newGadgetModel = (GadgetModel)MutableClone();
        newGadgetModel.Parent = gadget;
        return newGadgetModel;
    }

    public virtual Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player) => Task.CompletedTask;

    public void BreakMe()
    {
        AssertMutable();
        if (IsAllowedAsTempGadget)
        {
            Parent?.GadgetId = nameof(BrokenGadget);
        }
        else
        {
            throw new InvalidProgramException("Breakable Gadgets cannot be temporary");
        }
        
    }
}