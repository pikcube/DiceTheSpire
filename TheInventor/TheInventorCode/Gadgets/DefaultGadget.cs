namespace TheInventor.TheInventorCode.Gadgets;

public class DefaultGadget() : GadgetModel(nameof(DefaultGadget))
{
    public override bool IsAllowedAsTempGadget => false;
}