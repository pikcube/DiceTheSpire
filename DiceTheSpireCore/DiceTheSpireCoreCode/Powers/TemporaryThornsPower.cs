using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

  
public abstract class TemporaryThornsPower : DiceTheSpireCorePower, ITemporaryPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public abstract AbstractModel OriginModel { get; }
    public PowerModel InternallyAppliedPower => ModelDb.Power<ThornsPower>();

    private bool _shouldIgnoreNextInstance;
    protected virtual bool IsPositive => true;
    private int Sign => !this.IsPositive ? -1 : 1;

    public override async Task BeforeApplied(
        Creature target,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (this._shouldIgnoreNextInstance)
        {
            this._shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<ThornsPower>((PlayerChoiceContext)new ThrowingPlayerChoiceContext(), target,  amount, applier, cardSource, true);
        }
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        TemporaryThornsPower temporaryThornsPower = this;
        if (amount == temporaryThornsPower.Amount || power != temporaryThornsPower)
            return;
        if (temporaryThornsPower._shouldIgnoreNextInstance)
        {
            temporaryThornsPower._shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<ThornsPower>(choiceContext, temporaryThornsPower.Owner, amount, applier, cardSource, true);
        }
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {

        TemporaryThornsPower power = this;
        if (!participants.Contains(power.Owner) || side != power.Owner.Side)
        {
            return;
        }
        power.Flash();
        await PowerCmd.Remove(power);
        await PowerCmd.Apply<ThornsPower>(choiceContext, power.Owner, -power.Amount, power.Owner, null);
    }

    public void IgnoreNextInstance() => this._shouldIgnoreNextInstance = true;
}