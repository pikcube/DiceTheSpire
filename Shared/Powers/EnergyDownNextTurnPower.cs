using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.Shared.Powers;

public class EnergyDownNextTurnPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player)
        {
            return;
        }

        if (Amount == 1)
        {
            await PlayerCmd.LoseEnergy(1, player);
            await PowerCmd.Remove(this);
            return;
        }

        await PowerCmd.Decrement(this);
    }
}