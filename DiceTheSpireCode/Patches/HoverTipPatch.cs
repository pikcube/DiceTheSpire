using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Inventor;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpire.DiceTheSpireCode.Inventor.Token;
using DiceTheSpire.DiceTheSpireCode.Utility;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

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