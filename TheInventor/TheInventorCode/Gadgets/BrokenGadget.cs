using BaseLib.Abstracts;

namespace TheInventor.TheInventorCode.Gadgets;

public class BrokenGadget() : GadgetModel(nameof(BrokenGadget))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.None;
    public override bool IsAllowedAsTempGadget => false;
}