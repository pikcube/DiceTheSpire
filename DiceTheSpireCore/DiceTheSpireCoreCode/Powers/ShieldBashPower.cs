using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers
{

    public class ShieldBashPower : DiceTheSpireCorePower
    {

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
        {
            if (Owner.Player is null || Owner.Player.PlayerCombatState is null)
            {
                return;
            }
            ShieldBashPower shieldBashPower = this;
            shieldBashPower.Flash();
            await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), shieldBashPower.Owner, Amount, Applier, null);

            return;
        }
          
    }

}
