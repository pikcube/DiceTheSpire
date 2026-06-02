using BaseLib.Abstracts;

namespace TheInventor.TheInventorCode.Gadgets;

public class DefaultGadget() : GadgetModel(nameof(DefaultGadget))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.None;
    public override bool IsAllowedAsTempGadget => false;
}