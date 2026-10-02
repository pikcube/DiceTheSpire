using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Powers;

public class NextTurnVulnerablePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == Owner.Side)
        {
            return;
        }

        if (Amount == 1)
        {
            HookPlayerChoiceContext context = new(this, LocalContext.NetId ?? 0, CombatState, GameActionType.Combat);
            await VulnerablePower.ApplyAsync(context, Owner, 1, Applier, null);
            await PowerCmd.Remove(this);
        }
        else
        {
            await PowerCmd.Decrement(this);
        }
    }
}