using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Inventor.Gadgets;

public class DefaultGadget() : GadgetModel(nameof(DefaultGadget))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.None;
    public override bool IsAllowedAsTempGadget => false;

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player) => Task.CompletedTask;
}