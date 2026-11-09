using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class MidnightCharmPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyXValue(CardModel card, int originalValue)
    {
        return Owner != card.Owner.Creature ? originalValue : originalValue + Amount;
    }
}