using DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheThief.TheThiefCode.Powers;

public class WreckingBallPower : TheThiefPower, IAfterCardCountsDownListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task AfterCardCountsDownAsync(RunState runState, CardModel countdownCard)
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), CombatState.Enemies, Amount,
            ValueProp.Unpowered | ValueProp.Unblockable, Owner, countdownCard);
    }
}