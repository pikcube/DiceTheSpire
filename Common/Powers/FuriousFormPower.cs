using DiceTheSpire.Common.Listeners;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Powers;
public class FuriousFormPower : DiceTheSpireCorePower, IModifyFuryPlayCountListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public void ModifyFuryPlayCount(FuryPower furyPower, CardModel card, ref int furyCount)
    {
        if (furyPower.Owner == Owner)
        {
            furyCount += Amount;
        }
    }
}
