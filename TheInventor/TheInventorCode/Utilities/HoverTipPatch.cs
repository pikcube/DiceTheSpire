using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Utilities;

public class HoverTipPatch() : CustomSingletonModel(HookType.Run), IModifyHoverTipsListener
{
    public void ModifyCardHoverTips(CardModel sender, HoverTipEventArgs e)
    {
        if (sender.Owner.Character is not Character.TheInventor || sender is TheInventorCard)
        {
            return;
        }

        string id = ScrapManager.GetDefaultGadget(sender);

        GadgetModel gadgetModel = ScrapManager.AllGadgets[id];

        e.NewHoverTips.Add(new HoverTip(gadgetModel.Title, gadgetModel.Description, ModelDb.Power<GadgetPower>().Icon));
    }
}