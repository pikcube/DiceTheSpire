using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace TheInventor.TheInventorCode.Gadgets;

public class Harvest() : GadgetModel(nameof(Harvest))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Run;
    public override bool IsAllowedAsTempGadget => false;

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (Parent?.Owner != player)
        {
            return false;
        }

        if (Parent?.Owner.RunState.CurrentRoom == null)
        {
            return false;
        }

        for (int n = 0; n < 6; ++n)
        {
            GoldReward goldReward = new GoldReward(5 + 10 * n, 15 + 10 * n, player);
            goldReward.Populate();
            rewards.Add(goldReward);
        }

        BreakMe();
        return true;
    }
}