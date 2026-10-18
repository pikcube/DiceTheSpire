using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class KnockoutPunchQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => KnockoutPunch.Create();
}

//public class KnockoutPunchQuest() : TheWarriorCard(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedDamage(12, Bonus, 12)];

//    private static decimal Bonus(CardModel card, Creature? target)
//    {
//        return card.RunState?.CurrentRoom?.RoomType is RoomType.Boss or RoomType.Elite ? 2 : 0;
//    }

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        ArgumentNullException.ThrowIfNull(cardPlay.Target);

//            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
//            .FromCard(this, cardPlay)
//            .Targeting(cardPlay.Target)
//            .WithHitFx(VfxCmd.bluntPath)
//            .Execute(choiceContext);

//    }
//    protected override void OnUpgrade()
//    {
//        DynamicVars.CalculationBase.UpgradeValueBy(2);
//        DynamicVars.ExtraDamage.UpgradeValueBy(2);
//    }

//}
