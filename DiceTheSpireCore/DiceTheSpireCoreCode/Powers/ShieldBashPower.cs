using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class ShieldBashPower : DiceTheSpireCorePower
{

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (Owner.Player?.PlayerCombatState is null)
        {
            return;
        }
        ShieldBashPower shieldBashPower = this;
        shieldBashPower.Flash();
        await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), shieldBashPower.Owner, Amount, Applier, null);
    }
          
}