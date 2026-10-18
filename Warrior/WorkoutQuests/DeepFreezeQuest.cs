using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class DeepFreezeQuest() : WorkoutQuestCard
{
    public override CardModel RewardCard => DeepFreeze.Create();
}

//public class DeepFreezeQuest() : TheWarriorCard(2, CardType.Skill, CardRarity.Token, TargetType.AllEnemies)
//{
//    public override bool GainsBlock => true;
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FreezePower>()];
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(25, BlockProps.card), .. RangeVars.Make(1, 1), new PowerVar<FreezePower>(1)];
//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {

//        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

//        if (RunState is null || CombatState is null)
//        {
//            return;
//        }
//        await PowerCmd.Apply<FreezePower>(choiceContext, CombatState.Enemies, 1, Owner.Creature, cardPlay.Card);

//    }

//    protected override void OnUpgrade()
//    {
//        base.OnUpgrade();
//        DynamicVars.Block.UpgradeValueBy(5);

//    }

//}