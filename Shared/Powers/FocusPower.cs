using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace DiceTheSpire.Shared.Powers;
public class FocusPower : TheInventorPower
{ 
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player != Owner.Player ? count : count + Amount;
    }
}