using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace DiceTheSpire.Inventor.Powers;


public class PlasmaCannonPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner.Player is not null)
        {
            ScrapManager.DecreaseModification(Owner.Player);
        }

        return Task.CompletedTask;
    }
}