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

public class NextTurnVulnerablePower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == Owner.Side)
        {
            return;
        }

        HookPlayerChoiceContext context = new(this, LocalContext.NetId ?? 0, CombatState, GameActionType.Combat);
        await VulnerablePower.ApplyAsync(context, Owner, Amount, Applier, null);
        await PowerCmd.Remove(this);
    }
}