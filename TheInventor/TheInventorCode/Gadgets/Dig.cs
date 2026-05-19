using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Rewards;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class Dig() : AbstractGadget(nameof(Dig))
{
    public override bool TryModifyRestSiteHealRewards(Player player, List<Reward> rewards, bool isMimicked)
    {
        if (Parent?.Owner != player)
        {
            return false;
        }

        rewards.Add(new RelicReward(RelicFactory.PullNextRelicFromFront(Parent.Owner).ToMutable(), Parent.Owner));

        return true;
    }
}