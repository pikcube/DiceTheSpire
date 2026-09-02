using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Relics;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

public class ByrdNuggets() : GadgetModel(nameof(ByrdNuggets))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Run;

    public override async Task OnPickupAsync()
    {
        if (Parent?.Owner is null)
        {
            return;
        }

        await CreatureCmd.GainMaxHp(Parent.Owner.Creature, 8);
        Byrdpip? relic = Parent.Owner.GetRelic<Byrdpip>();
        if (relic is not null)
        {
            await RelicCmd.Remove(relic);
        }

        BreakMe();
    }

    public override bool IsAllowedAsTempGadget => false;
}