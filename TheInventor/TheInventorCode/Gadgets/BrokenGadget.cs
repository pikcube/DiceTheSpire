namespace TheInventor.TheInventorCode.Gadgets;

public class BrokenGadget() : GadgetModel(nameof(BrokenGadget))
{
    public override bool IsAllowedAsTempGadget => false;
}