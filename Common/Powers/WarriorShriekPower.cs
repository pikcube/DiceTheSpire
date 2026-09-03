using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Common.Powers;
public class WarriorShriekPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (Owner.Player?.PlayerCombatState is null)
        {
            return;
        }

        Flash();
        await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Applier, null);
    }

}