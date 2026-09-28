using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Inventor.Gadgets;

[UsedImplicitly]
public class StinkyGadget() : GadgetModel(nameof(StinkyGadget))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.None;
    public override bool IsAllowedAsTempGadget => false;
    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player) => Task.CompletedTask;
}