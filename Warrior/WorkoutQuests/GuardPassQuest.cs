using MegaCrit.Sts2.Core.Models;
using DiceTheSpire.Warrior.WorkoutRewards;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class GuardPassQuest() : WorkoutQuestCard
{
    public override CardModel RewardCard => GuardPass.Create();
}

//public class GuardPassQuest() : TheWarriorCard(2, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedDamage(0, Bonus)];
//    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
//    private static decimal Bonus(CardModel card, Creature? arg2)
//    {

//        if (card.Owner.PlayerCombatState is null)
//        {
//            return 0;
//        }

//        int damage = card.Owner.Creature.Block * 4;

//        return damage;
//    }

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        if (Owner.PlayerCombatState is null || cardPlay.Target is null)
//        {
//            return;
//        }

//        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
//            .FromCard(this, cardPlay)
//            .Targeting(cardPlay.Target)
//            .WithHitFx(VfxCmd.slashPath)
//            .Execute(choiceContext);

//    }

//    protected override void OnUpgrade()
//    {
//        EnergyCost.UpgradeBy(-1);
//    }

//}