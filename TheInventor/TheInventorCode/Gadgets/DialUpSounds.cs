using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class DialUpSounds() : GadgetModel(nameof(DialUpSounds))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public bool IsUsedUp { get; set; }

    public override bool IsAllowedAsTempGadget => false;

    public override Task BeforeCombatStart()
    {
        IsUsedUp = false;
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (IsUsedUp || Parent?.Owner.Creature is null)
        {
            return;
        }

        await BufferPower.ApplyAsync(choiceContext, Parent.Owner.Creature, Power,
            Parent.Owner.Creature, null);

        IsUsedUp = true;
    }

    public override async Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner.Creature is null)
        {
            return;
        }

        await BufferPower.ApplyAsync(choiceContext, Parent.Owner.Creature, Power, Parent.Owner.Creature, null);
    }
}