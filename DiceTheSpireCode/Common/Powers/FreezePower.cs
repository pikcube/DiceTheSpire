using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

[UsedImplicitly]
public class FreezePower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override decimal ModifyBlockMultiplicative(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay)
    {
        return target == Owner ? 0 : 1;
    }

    public override async Task AfterModifyingBlockAmount(decimal modifiedAmount, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (Owner.Block != 0)
        {
            return;
        }

        if (Owner.Player is not null)
        {
            await Hook.AfterBlockBroken(CombatState, new HookPlayerChoiceContext(Owner.Player, Owner.Player.NetId, GameActionType.Combat), Owner, null);
        }
        else
        {
            await Hook.AfterBlockBroken(CombatState, new BlockingPlayerChoiceContext(), Owner, null);
        }
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        return Owner.Side == side ? PowerCmd.Remove(this) : Task.CompletedTask;
    }
}