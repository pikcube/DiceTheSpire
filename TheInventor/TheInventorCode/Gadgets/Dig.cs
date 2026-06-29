using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rewards;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class Dig() : GadgetModel(nameof(Dig))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Run;
    public override bool IsAllowedAsTempGadget => false;

    public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(Player player, IReadOnlyList<LocString> currentExtraText)
    {
        return player == Parent?.Owner ? [..currentExtraText, new LocString("rest_site_ui", $"OPTION_DIG.description")] : currentExtraText;
    }

    public override bool TryModifyRestSiteHealRewards(Player player, List<Reward> rewards, bool isMimicked)
    {
        if (Parent?.Owner != player)
        {
            return false;
        }

        for (int n = 0; n < Power; ++n)
        {
            rewards.Add(new RelicReward(RelicFactory.PullNextRelicFromFront(Parent.Owner).ToMutable(), Parent.Owner));
        }

        return true;
    }
}