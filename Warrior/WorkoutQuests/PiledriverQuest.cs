using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class PiledriverQuest() : TheWarriorCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None), IWorkoutQuest
{
    public CardModel RewardCard => throw new NotImplementedException();
}

//public class PiledriverQuest() : TheWarriorCard(3, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
//{
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(25M, DamageProps.card), new PowerVar<VulnerablePower>(5)];

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        ArgumentNullException.ThrowIfNull(cardPlay.Target);

//        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
//            .FromCard(this, cardPlay)
//            .Targeting(cardPlay.Target)
//            .WithHitFx(VfxCmd.bluntPath)
//            .Execute(choiceContext);

//        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.IntValue,
//            Owner.Creature, cardPlay.Card);

//    }

//    protected override void OnUpgrade()
//    {
//        DynamicVars.Damage.UpgradeValueBy(5);
//        DynamicVars.Vulnerable.UpgradeValueBy(2);

//    }

//}
