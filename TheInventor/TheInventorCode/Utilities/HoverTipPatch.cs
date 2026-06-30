using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Cards.Token;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Utilities;

public class HoverTipPatch() : CustomSingletonModel(HookType.Run), IModifyHoverTipsListener
{
    public void ModifyCardHoverTips(CardModel sender, HoverTipEventArgs e)
    {
        if (sender.Owner.Character is not Character.TheInventor || sender is TheInventorCard || !TheInventorCard.ShowGadgetTips(sender))
        {
            return;
        }

        string id = ScrapManager.GetDefaultGadget(sender);

        GadgetModel gadgetModel = ScrapManager.AllGadgets[id];
        GadgetCard gadgetCard = GadgetCard.Create();
        gadgetCard.SetVars(gadgetModel, sender);
        e.NewHoverTips.Add(new CardHoverTip(gadgetCard));
    }
}