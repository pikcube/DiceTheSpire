using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Warrior.WorkoutRewards;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;

public class CartwheelQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => Cartwheel.Create();
}


//public class CartwheelQuest() : TheWarriorCard(3, CardType.Skill, CardRarity.Token, TargetType.Self), IAfterRerollListener
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
//    }
//    public async Task AfterRerollAsync(PlayerChoiceContext choiceContext, CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
//    {
//        if (card == this)
//        {
//            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
//        }
//    }
//    protected override void OnUpgrade()
//    {
//        DynamicVars.Cards.UpgradeValueBy(1);
//    }

//}
