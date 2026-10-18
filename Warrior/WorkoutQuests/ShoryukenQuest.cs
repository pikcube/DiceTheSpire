using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class ShoryukenQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => Shoryuken.Create();
}

//public class ShoryukenQuest() : TheWarriorCard(1, CardType.Power, CardRarity.Token, TargetType.Self)
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ShoryukenPower>(1M)];
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        await PowerCmd.Apply<ShoryukenPower>(choiceContext, Owner.Creature, DynamicVars.Power<ShoryukenPower>().IntValue, Owner.Creature, this);

//    }
//    protected override void OnUpgrade()
//    {
//        AddKeyword(CardKeyword.Innate);
//    }

//}
