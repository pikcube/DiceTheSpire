using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Listeners;

public interface IModifyRerollListener
{
    public void ModifyRerollRange(CardModel card, ref int minimum, ref int maximum);
}