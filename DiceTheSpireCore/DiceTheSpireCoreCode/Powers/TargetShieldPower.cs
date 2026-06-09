using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;


    public class TargetShieldPower : DiceTheSpireCorePower
    {

        protected override IEnumerable<IHoverTip> ExtraHoverTips => (IEnumerable<IHoverTip>)HoverTipFactory.FromPower<DexterityPower>();
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        //AfterSideTurnStart
        public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            TargetShieldPower targetShieldPower = this;
            if (!participants.Contains(targetShieldPower.Owner))
                return;
            targetShieldPower.Flash(); 
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), targetShieldPower.Owner, targetShieldPower.Amount, targetShieldPower.Owner, null);

        }
    }

