using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IModifyRerollListener
{
    public void ModifyRerollRange(CardModel card, ref int minimum, ref int maximum);
}