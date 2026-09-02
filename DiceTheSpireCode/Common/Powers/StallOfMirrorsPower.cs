using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

[UsedImplicitly]
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
        StallOfMirrorsHelper.CurrentStall.Set(Owner.Player, Amount);
        return Task.CompletedTask;
    }
}