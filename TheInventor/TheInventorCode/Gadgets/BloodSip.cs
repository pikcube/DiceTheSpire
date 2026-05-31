using MegaCrit.Sts2.Core.Commands;

namespace TheInventor.TheInventorCode.Gadgets;

public class BloodSip() : GadgetModel(nameof(BloodSip))
{
    public override async Task BeforeCombatStart()
    {
        if (Parent?.Owner is null)
        {
            return;
        }

        await CreatureCmd.Heal(Parent.Owner.Creature, 6);
        BreakMe();
    }

    public override bool IsAllowedAsTempGadget => false;
}