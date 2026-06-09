using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;


    public class targetShieldPower : DiceTheSpireCorePower
    {

        protected override IEnumerable<IHoverTip> ExtraHoverTips => (IEnumerable<IHoverTip>)HoverTipFactory.FromPower<DexterityPower>();
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        //AfterSideTurnStart
        public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            targetShieldPower targetShieldPower = this;
            if (!participants.Contains<Creature>(targetShieldPower.Owner))
                return;
            targetShieldPower.Flash();
        DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), targetShieldPower.Owner, targetShieldPower.Amount, targetShieldPower.Owner, (CardModel)null);

        }
    }

