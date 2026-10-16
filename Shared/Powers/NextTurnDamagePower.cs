using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Powers;

public class NextTurnDamagePower : NextTurnPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override TickTime Tick => TickTime.Start;
    public override CombatSide TickSide => CombatSide.Player;

    public override PowerInstanceType NextTurnInstanceType => PowerInstanceType.Instanced;

    public override async Task OnDelayOverAsync(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, Owner, Amount, DamageProps.nonCardUnpowered, null, null);
    }
}