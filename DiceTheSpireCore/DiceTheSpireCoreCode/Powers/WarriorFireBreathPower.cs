using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
public class WarriorFireBreathPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if(power.Owner is null || power is null || Owner is null)
        {
            return;
        }
        if (power is FuryPower) //and power.Owner == Owner
        {
            await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), Owner, this.Amount, Applier, null);
        }
    }


}
