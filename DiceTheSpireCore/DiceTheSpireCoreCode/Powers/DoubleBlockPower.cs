using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
public class DoubleBlockPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (Owner.Player?.PlayerCombatState is null)
        {
            return;
        }
        DoubleBlockPower doubleBlockPower = this;
        doubleBlockPower.Flash();
        
    }
    public override Decimal ModifyBlockMultiplicative(Creature target, Decimal block,ValueProp props,CardModel? cardSource,CardPlay? cardPlay)
    {
        return target.IsMonster || !props.IsCardOrMonsterMove() || cardSource != null && cardSource.Owner.Creature != this.Owner ? 1M : 2M;
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Side)
        {
            return;
        }

        await PowerCmd.Remove(this);
    }
}