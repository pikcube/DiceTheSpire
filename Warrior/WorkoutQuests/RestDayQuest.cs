using MegaCrit.Sts2.Core.Entities.Cards;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class RestDayQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => RestDay.Create();
}

//{
//    public class RestDayQuest() : TheWarriorCard(3, CardType.Skill, CardRarity.Token, TargetType.Self)
//    {
//        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BlockNextTurnPower>(40)];
//        public override bool GainsBlock => true;
//        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//        {
//            await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Power<BlockNextTurnPower>().IntValue, Owner.Creature, this);

//        }
//        protected override void OnUpgrade()
//        {
//            DynamicVars.Power<BlockNextTurnPower>().UpgradeValueBy(10);
//        }
//    }
//}


