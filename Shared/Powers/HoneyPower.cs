using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class HoneyPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyXValue(CardModel card, int originalValue)
    {
        if (Owner != card.Owner.Creature)
        {
            return originalValue;
        }

        int newValue = originalValue + Amount;
        PowerCmd.Remove(this);
        return newValue;
    }
}