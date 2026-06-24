using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public abstract class TemporaryReducePower: DiceTheSpireCorePower, ITemporaryPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private bool _shouldIgnoreNextInstance;
    public void IgnoreNextInstance() => _shouldIgnoreNextInstance = true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..GetCardTip()];

    private CardModel? CardSource { get; set; }

    private IEnumerable<IHoverTip> GetCardTip()
    {
        if (CardSource is not null)
        {
            yield return HoverTipFactory.FromCard(CardSource);
        }
    }

    public abstract AbstractModel OriginModel { get; }
    public PowerModel InternallyAppliedPower => ModelDb.Power<ReducePower>();

    public override async Task BeforeApplied(
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        CardSource = cardSource;
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

            HookPlayerChoiceContext context = new(p, LocalContext.NetId ?? 0, GameActionType.Combat);
            await PowerCmd.Apply<ReducePower>(context, target, amount, applier, cardSource, true);
        }
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (amount == Amount || power != this)
            return;
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<ReducePower>(choiceContext, Owner, amount, applier, cardSource, true);
        }
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner) || side != Owner.Side)
        {
            return;
        }
        Flash();
        await PowerCmd.Remove(this);
        await PowerCmd.Apply<ReducePower>(choiceContext, Owner, -Amount, Owner, null);
    }
}