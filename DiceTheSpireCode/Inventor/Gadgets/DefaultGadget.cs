using BaseLib.Abstracts;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

public class DefaultGadget() : GadgetModel(nameof(DefaultGadget))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.None;
    public override bool IsAllowedAsTempGadget => false;
}