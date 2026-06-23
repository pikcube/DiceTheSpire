using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Utilities;

public class HoverTipPatch() : CustomSingletonModel(HookType.Run), IModifyHoverTipsListener
{
    public void ModifyCardHoverTips(CardModel sender, HoverTipEventArgs e)
    {
        if (sender.Owner.Character is not Character.TheInventor || sender is TheInventorCard)
        {
            return;
        }

        string id = Gadget.GetDefaultGadget(sender);

        GadgetModel gadgetModel = Gadget.AllGadgets[id];

        e.NewHoverTips.Add(new HoverTip(gadgetModel.Title, gadgetModel.Description, ModelDb.Relic<Gadget>().Icon));
    }
}