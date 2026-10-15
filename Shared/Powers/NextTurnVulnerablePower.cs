using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Powers;

public class NextTurnVulnerablePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VulnerablePower>(1)];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == Owner.Side)
        {
            return;
        }

        if (Amount == 1)
        {
            HookPlayerChoiceContext context = new(this, LocalContext.NetId ?? 0, CombatState, GameActionType.Combat);
            await VulnerablePower.ApplyAsync(context, Owner, DynamicVars.Vulnerable.IntValue, Applier, null);
            await PowerCmd.Remove(this);
        }
        else
        {
            await PowerCmd.Decrement(this);
        }
    }

    public static async Task ApplyAsync(PlayerChoiceContext choiceContext,
        IEnumerable<Creature> targets,
        decimal amount,
        int potency,
        Creature? applier,
        CardModel? cardSource,
        bool silent = false)
    {
        IReadOnlyList<NextTurnVulnerablePower> powers = await NextTurnVulnerablePower.ApplyAsync(choiceContext, targets, amount, applier, cardSource, silent);
        foreach (NextTurnVulnerablePower power in powers)
        {
            power.DynamicVars.Vulnerable.BaseValue = potency;
        }
    }

    public static async Task ApplyAsync(PlayerChoiceContext choiceContext,
        Creature target,
        decimal amount,
        int potency,
        Creature? applier,
        CardModel? cardSource,
        bool silent = false)
    {
        NextTurnVulnerablePower? power = await NextTurnVulnerablePower.ApplyAsync(choiceContext, target, amount, applier, cardSource, silent);
        power?.DynamicVars.Vulnerable.BaseValue = potency;
    }
}