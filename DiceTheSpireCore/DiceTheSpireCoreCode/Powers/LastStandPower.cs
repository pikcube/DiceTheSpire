using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class LastStandPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool ShouldDie(Creature creature) => creature != this.Owner;

    public override async Task AfterPreventingDeath(Creature creature)
    {
            await CreatureCmd.Heal(Owner, 1M, false);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
    IEnumerable<Creature> owner)
    {
            if (side == Owner.Side)
            {
                return;
            }
            LastStandPower lastStandPower = this;
            lastStandPower.Flash();
            if (lastStandPower.Amount == 1)
            {
                await PowerCmd.Decrement(this);
                await CreatureCmd.Kill(Owner);
            }
            else
            {
                await PowerCmd.Decrement(this);
            }
            lastStandPower.Flash();
        
    }
}

