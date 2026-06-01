using BaseLib.Abstracts;
using JetBrains.Annotations;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class RottenGadget() : GadgetModel(nameof(RottenGadget))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.None;
    public override bool IsAllowedAsTempGadget => false;
}