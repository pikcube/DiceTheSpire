using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;


public abstract class TemporaryThornsPower : DiceTheSpirePower, ITemporaryPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public abstract AbstractModel OriginModel { get; }
    public PowerModel InternallyAppliedPower => ModelDb.Power<ThornsPower>();

    private bool _shouldIgnoreNextInstance;

    public override async Task BeforeApplied(
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        Player? p = target.Player ?? applier?.Player ?? cardSource?.Owner ?? target.CombatState?.Players[0];
        if (p is null)
        {
            return;
        }
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<ThornsPower>(new HookPlayerChoiceContext(p, LocalContext.NetId ?? 0, GameActionType.Combat), target, amount, applier, cardSource, true);
        }
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        TemporaryThornsPower temporaryThornsPower = this;
        if (amount == temporaryThornsPower.Amount || power != temporaryThornsPower)
        {
            return;
        }

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

    public void IgnoreNextInstance() => _shouldIgnoreNextInstance = true;
}