using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.WorkoutRewards;
public class DeepFreeze() : TheWarriorCard(2, CardType.Skill, CardRarity.Token, TargetType.AllEnemies)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FreezePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(20, BlockProps.card), .. RangeVars.Make(1, 1), new PowerVar<FreezePower>(1)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        if (RunState is null || CombatState is null)
        {
            return;
        }
        await PowerCmd.Apply<FreezePower>(choiceContext, CombatState.Enemies, 1, Owner.Creature, cardPlay.Card);

    }

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Block.UpgradeValueBy(5);

    }

}