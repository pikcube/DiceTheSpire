using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

public class BloodSip() : GadgetModel(nameof(BloodSip))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override decimal PowerBase => 6;

    public override async Task BeforeCombatStart()
    {
        if (Parent?.Owner is null)
        {
            return;
        }

        await CreatureCmd.Heal(Parent.Owner.Creature, Power);
    }

    public override async Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner is null)
        {
            return;
        }

        await CreatureCmd.Heal(Parent.Owner.Creature, Power);
    }

    public override bool IsAllowedAsTempGadget => false;
}