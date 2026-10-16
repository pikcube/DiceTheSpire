using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Powers;

public class NextTurnVulnerablePower : NextTurnPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override TickTime Tick => TickTime.Start;
    public override CombatSide TickSide => CombatSide.Player;

    public override async Task OnDelayOverAsync(PlayerChoiceContext choiceContext)
    {
        await VulnerablePower.ApplyAsync(choiceContext, Owner, Amount, Applier, null);
    }
}