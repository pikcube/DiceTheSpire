using BaseLib.Extensions;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.WorkoutRewards
{
    public class RestDay() : TheWarriorCard(3, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BlockNextTurnPower>(50)];
        public override bool GainsBlock => true;
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Power<BlockNextTurnPower>().IntValue, Owner.Creature, this);

        }
        protected override void OnUpgrade()
        {
            DynamicVars.Power<BlockNextTurnPower>().UpgradeValueBy(10);
        }
    }
}


