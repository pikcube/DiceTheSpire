using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Common.Powers;
public class WarriorIceBreathPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
        {
            return;
        }
        if (Owner.HasPower<FuryPower>())
        {
            await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
        }
    }
}
