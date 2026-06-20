using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public abstract class TemporaryReducePower: DiceTheSpireCorePower, ITemporaryPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private bool _shouldIgnoreNextInstance;
    public void IgnoreNextInstance() => _shouldIgnoreNextInstance = true;

    public abstract AbstractModel OriginModel { get; }
    public PowerModel InternallyAppliedPower => ModelDb.Power<ReducePower>();

    public override async Task BeforeApplied(
        Creature target,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<ReducePower>(new ThrowingPlayerChoiceContext(), target, amount, applier, cardSource, true);
        }
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        TemporaryReducePower temporaryReducePower = this;
        if (amount == temporaryReducePower.Amount || power != temporaryReducePower)
            return;
        if (temporaryReducePower._shouldIgnoreNextInstance)
        {
            temporaryReducePower._shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<ReducePower>(choiceContext, temporaryReducePower.Owner, amount, applier, cardSource, true);
        }
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {

        TemporaryReducePower power = this;
        if (!participants.Contains(power.Owner) || side != power.Owner.Side)
        {
            return;
        }
        power.Flash();
        await PowerCmd.Remove(power);
        await PowerCmd.Apply<ReducePower>(choiceContext, power.Owner, -power.Amount, power.Owner, null);
    }
}