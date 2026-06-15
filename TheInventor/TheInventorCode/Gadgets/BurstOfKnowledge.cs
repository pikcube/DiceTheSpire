using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace TheInventor.TheInventorCode.Gadgets;

public class BurstOfKnowledge() : GadgetModel(nameof(BurstOfKnowledge))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Run;
    public override bool IsAllowedAsTempGadget => false;

    public override decimal PowerBase => 2;

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

        for (int n = 0; n < 2; ++n)
        {
            rewards.Add(new CardReward(CardCreationOptions.ForRoom(Parent.Owner, Parent.Owner.RunState.CurrentRoom.RoomType), 3, Parent.Owner));
        }

        BreakMe();
        return true;
    }
}