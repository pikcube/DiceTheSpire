using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Gadgets;

[CustomLocTable("gadgets.json")]
public abstract class AbstractGadget : AbstractModel, ICustomModel
{ 
    public Gadget? Parent { get; set; }
    protected AbstractGadget(string gadgetId)
    {
        GadgetId = gadgetId;
        Gadget.AllGadgets[GadgetId] = this;
    }
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
    public virtual IEnumerable<string> GadgetLocStrings => [Title.GetRawText(), $"{Description.GetRawText()}\n\n"];

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
    public AbstractGadget GetMutable(Gadget gadget)
    {
        AbstractGadget newGadget = (AbstractGadget)MutableClone();
        newGadget.Parent = gadget;
        return newGadget;
    }

    public virtual Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player) => Task.CompletedTask;

    public void BreakMe()
    {
        AssertMutable();
        Parent?.GadgetId = nameof(BrokenGadget);
    }
}