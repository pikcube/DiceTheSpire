using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class SomersaultQuest() : WorkoutQuestCard
{
    public override CardModel RewardCard => Somersault.Create();
}

//public class SomersaultQuest() : TheWarriorCard(1, CardType.Skill, CardRarity.Token, TargetType.Self)
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Rummage)];
//    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];
//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
//        await RummageCmd.RummageAsync(choiceContext, Owner, DynamicVars.Cards.IntValue, this);

//    }
//    protected override void OnUpgrade()
//    {
//        DynamicVars.Cards.UpgradeValueBy(1);
//    }

//}
