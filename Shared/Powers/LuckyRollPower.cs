using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;
public class LuckyRollPower : DiceTheSpirePower, IModifyRerollListener
{

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public void ModifyRerollRange(CardModel card, ref int minimum, ref int maximum)
    {
        maximum = Math.Min(maximum, card.EnergyCost.GetAmountToSpend());
        maximum = Math.Max(maximum, minimum);
    }

}

