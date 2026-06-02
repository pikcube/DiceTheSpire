using BaseLib.Abstracts;

namespace TheInventor.TheInventorCode.Gadgets;

public class PersistenceOfMemory() : GadgetModel(nameof(PersistenceOfMemory))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.None;
    public override bool IsAllowedAsTempGadget => false;
}