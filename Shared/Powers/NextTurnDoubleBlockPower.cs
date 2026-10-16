using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Powers;

public class NextTurnDoubleBlockPower : AbstractNextTurnPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override TickTime Tick => TickTime.Start;
    public override CombatSide TickSide => CombatSide.Player;

    public override async Task OnDelayOverAsync(PlayerChoiceContext choiceContext)
    {
        await DoubleBlockPower.ApplyAsync(choiceContext, Owner, Amount, Applier, null);
    }
}