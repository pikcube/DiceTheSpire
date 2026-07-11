using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Cards.Token;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Patches;

public class HoverTipPatch() : CustomSingletonModel(HookType.Run), IModifyHoverTipsListener
{
    public void ModifyCardHoverTips(CardModel sender, HoverTipEventArgs e)
    {
        if (sender.Owner.Character is not Character.TheInventor || sender is TheInventorCard || sender is GadgetCard || !TheInventorCard.ShowGadgetTips(sender))
        {
            return;
        }

        string id = ScrapManager.GetDefaultGadget(sender);

        GadgetModel gadgetModel = ScrapManager.AllGadgets[id];
        GadgetCard gadgetCard = GadgetCard1.Create();
        gadgetCard.SetVars(gadgetModel);
        e.NewHoverTips.Add(new CardHoverTip(gadgetCard));
    }
}