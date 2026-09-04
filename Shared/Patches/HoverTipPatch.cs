using BaseLib.Abstracts;
using DiceTheSpire.Inventor;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Inventor.Token;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.Shared.Patches;

public class HoverTipPatch() : CustomSingletonModel(HookType.Run), IModifyHoverTipsListener
{
    public void ModifyCardHoverTips(CardModel sender, HoverTipEventArgs e)
    {
        if (sender.Owner.Character is not TheInventor || sender is TheInventorCard || sender is GadgetCard || !TheInventorCard.ShowGadgetTips(sender))
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