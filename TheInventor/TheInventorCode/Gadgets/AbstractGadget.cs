using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Localization;
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
    public string GadgetId { get; }
    public string GadgetText => string.Join(": ", GadgetLocStrings);
    public virtual IEnumerable<string> GadgetLocStrings => [Title.GetRawText(), $"{Description.GetRawText()}\n\n"];

    public LocString Title
    {
        get
        {
            field ??= new LocString("gadgets", Id.Entry + ".title");
            return field;
        }
    }
    public LocString Description
    {
        get
        {
            field ??= new LocString("gadgets", Id.Entry + ".description");
            return field;
        }
    }
    public AbstractGadget GetMutable(Gadget gadget)
    {
        AbstractGadget newGadget = (AbstractGadget)MutableClone();
        newGadget.Parent = gadget;
        return newGadget;
    }
}