using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class StunlockQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => Stunlock.Create();
}

//public class StunlockQuest() : TheWarriorCard(3, CardType.Skill, CardRarity.Token, TargetType.AnyEnemy)
//{
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [StunIntent.GetStaticHoverTip()];
//    protected override IEnumerable<DynamicVar> CanonicalVars => [.. RangeVars.Make(0, 2)];
//    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        ArgumentNullException.ThrowIfNull(cardPlay.Target);

//        await CreatureCmd.Stun(cardPlay.Target);
//    }

//    protected override void OnUpgrade()
//    {
//        DynamicVars.MaxRange.UpgradeValueBy(-2);
//    }

//}
