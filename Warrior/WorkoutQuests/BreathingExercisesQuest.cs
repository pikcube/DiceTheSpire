using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class BreathingExercisesQuest() : WorkoutQuestCard
{
    public override CardModel RewardCard => BreathingExercises.Create();
}

//public class BreathingExercisesQuest() : TheWarriorCard(0, CardType.Skill, CardRarity.Token, TargetType.Self), IFuryModifier
//{
//    public bool ShouldIgnoreFury => true;
//    public bool ShouldMaintainFury => true;
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryPower>(2M)];
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];
//    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, DynamicVars.Power<FuryPower>().IntValue, Owner.Creature, this);
//    }
//    protected override void OnUpgrade()
//    {
//        DynamicVars.Power<FuryPower>().UpgradeValueBy(1);
//    }

//}
