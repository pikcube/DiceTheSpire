using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;
using TheInventor.TheInventorCode.Cards.Rare;

namespace TheInventor.TheInventorCode.Powers;

public class StallOfMirrorsPower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner.Player is null)
        {
            return Task.CompletedTask;
        }
        StallOfMirrors.CurrentStall.Set(Owner.Player, Amount);
        return Task.CompletedTask;
    }
}