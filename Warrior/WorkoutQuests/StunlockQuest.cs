using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class StunlockQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => throw new NotImplementedException();
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
