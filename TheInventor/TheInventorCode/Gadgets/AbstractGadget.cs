using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
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
    public abstract string GadgetText { get; }
    public virtual string GadgetSuffix => "\n\n";
    public AbstractGadget GetMutable(Gadget gadget)
    {
        AbstractGadget newGadget = (AbstractGadget)MutableClone();
        newGadget.Parent = gadget;
        return newGadget;
    }
}