using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

public class BloodSip() : GadgetModel(nameof(BloodSip))
{
    public bool IsUsedUp { get; set; }
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override decimal PowerBase => 6;

    public override async Task BeforeCombatStart()
    {
        if (IsUsedUp || Parent?.Owner is null)
        {
            return;
        }

        await CreatureCmd.Heal(Parent.Owner.Creature, Power);
        IsUsedUp = true;
    }

    public override async Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (IsUsedUp || Parent?.Owner is null)
        {
            return;
        }

        await CreatureCmd.Heal(Parent.Owner.Creature, Power);
    }

    public override bool IsAllowedAsTempGadget => false;
}