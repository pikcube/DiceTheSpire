using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class SomersaultQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => throw new NotImplementedException();
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
