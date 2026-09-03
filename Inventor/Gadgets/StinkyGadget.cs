using BaseLib.Abstracts;
using JetBrains.Annotations;

namespace DiceTheSpire.Inventor.Gadgets;

[UsedImplicitly]
public class StinkyGadget() : GadgetModel(nameof(StinkyGadget))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.None;
    public override bool IsAllowedAsTempGadget => false;
}