using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Gadgets;

public abstract class AbstractGadget : AbstractModel, ICustomModel
{
    public Gadget? Parent { get; set; }

    protected AbstractGadget(string gadgetId)
    {
        GadgetId = gadgetId;
        Gadget.AllGadgets[GadgetId] = this;
    }
    public override bool ShouldReceiveCombatHooks => true;
    public string GadgetId { get; }

    public abstract StringVar GadgetName { get; }
    public abstract StringVar GadgetDescription { get; }

    public AbstractGadget GetMutable(Gadget gadget)
    {
        AbstractGadget newGadget = (AbstractGadget)MutableClone();
        newGadget.Parent = gadget;
        return newGadget;
    }
}