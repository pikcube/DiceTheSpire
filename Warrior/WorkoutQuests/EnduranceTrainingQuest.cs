using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class EnduranceTrainingQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => EnduranceTraining.Create();
}

//public class EnduranceTrainingQuest() : TheWarriorCard(0, CardType.Power, CardRarity.Token, TargetType.Self)
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(2M)];
//    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate];
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>(DynamicVars.Power<DexterityPower>().IntValue)];

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars.Dexterity.BaseValue, Owner.Creature, this);
//    }

//    protected override void OnUpgrade()
//    {
//        DynamicVars.Dexterity.UpgradeValueBy(1);
//    }
//}