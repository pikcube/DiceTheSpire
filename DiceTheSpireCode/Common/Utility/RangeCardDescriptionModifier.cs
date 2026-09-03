using DiceTheSpire.DiceTheSpireCode.Common.DynamicVars;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Utility;

public static class RangeCardDescriptionModifier
{
    public static void ModifyCardText(CardModel card, ref List<string> lines)
    {
        if (!RangeVars.TryGet(card, out int min, out int max))
        {
            return;
        }

        if(min == 0 && max == 3)
        {
            return;
        }

        if (min == max)
        {
            LocString text = new("card_keywords", $"{MainFile.ModPrefix}-RANGE.single");
            card.DynamicVars.AddTo(text);
            lines.Insert(0, text.GetFormattedText());
        }
        else
        {
            LocString text = new("card_keywords", $"{MainFile.ModPrefix}-RANGE.double");
            card.DynamicVars.AddTo(text);
            lines.Insert(0, text.GetFormattedText());
        }
    }
}