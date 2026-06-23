using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class HallOfMirrorsPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player.Creature == Owner)
        {
            return amount + Amount;
        }

        return amount;
    }
}