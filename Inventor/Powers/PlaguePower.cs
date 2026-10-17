using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Inventor.Powers;

public class PlaguePower : AbstractNextTurnPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override TickTime Tick => TickTime.Start;
    public override CombatSide TickSide => CombatSide.Player;

    public override async Task OnDelayOverAsync(PlayerChoiceContext choiceContext)
    {
        if (Owner.CombatState?.RunState is null)
        {
            return;
        }

        for(int n = 0; n < Amount; ++n)
        {
            await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, Owner.CombatState.RunState, Owner, Applier, null);
        }
    }
}