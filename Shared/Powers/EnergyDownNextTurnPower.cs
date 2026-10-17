using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.Shared.Powers;

public class EnergyDownNextTurnPower : AbstractNextTurnPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];


    public override TickTime Tick => TickTime.Start;
    public override CombatSide TickSide => CombatSide.Player;

    public override async Task OnDelayOverAsync(PlayerChoiceContext choiceContext)
    {
        if (Owner.Player is not null)
        {
            await PlayerCmd.LoseEnergy(Amount, Owner.Player);
        }
    }
}