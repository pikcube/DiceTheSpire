using JetBrains.Annotations;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class RottenGadget() : GadgetModel(nameof(RottenGadget))
{
    public override bool IsAllowedAsTempGadget => false;
}