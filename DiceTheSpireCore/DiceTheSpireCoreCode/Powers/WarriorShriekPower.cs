using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
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
        WarriorShriekPower shriekPower = this;
        shriekPower.Flash();
        await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), shriekPower.Owner, Amount, Applier, null);
    }

}