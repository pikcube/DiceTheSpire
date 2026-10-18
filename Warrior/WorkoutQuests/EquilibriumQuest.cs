using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class EquilibriumQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => Equilibrium.Create();
}

//public class EquilibriumQuest() : TheWarriorCard(3, CardType.Power, CardRarity.Token, TargetType.Self)
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<EquilibriumPower>(30), .. RangeVars.Make(0, 2)];
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<EquilibriumPower>()];

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        await EquilibriumPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<EquilibriumPower>().IntValue, Owner.Creature, this);
//    }
//    protected override void OnUpgrade()
//    {
//        DynamicVars.Power<EquilibriumPower>().UpgradeValueBy(10);
//    }

//}
