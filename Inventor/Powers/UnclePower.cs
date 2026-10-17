using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Powers;

public class UnclePower : AbstractNextTurnPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override TickTime Tick => TickTime.End;
    public override CombatSide TickSide => CombatSide.Player;

    protected override IEnumerable<DynamicVar> ExtraCanonicalVars =>
        [new DamageVar(90, DamageProps.nonCardHpLoss)];

    public override async Task OnDelayOverAsync(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, CombatState.Enemies, Amount, DynamicVars.Damage.Props, Owner, null, null);
    }
}