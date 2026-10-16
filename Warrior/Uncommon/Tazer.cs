using BaseLib.Extensions;
using DiceTheSpire.Shared.DynamicVars;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using DiceTheSpire.Shared.Extensions;

namespace DiceTheSpire.Warrior.Uncommon;

public class Tazer() : TheWarriorCard(3, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VigorPower>(4M), new PowerVar<WeakPower>(4), .. RangeVars.Make(0, 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(DynamicVars.Power<WeakPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (RunState is null || CombatState is null)
        {
            return;
        }
        await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.Enemies, DynamicVars.Weak.IntValue, Owner.Creature, cardPlay.Card);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars.Power<VigorPower>().IntValue, Owner.Creature, cardPlay.Card);
    }
    protected override void OnUpgrade()
    {
        //DynamicVars.Power<VigorPower>().UpgradeValueBy(2);
        //DynamicVars.Weak.UpgradeValueBy(1);
        DynamicVars.MaxRange.UpgradeValueBy(-2);
    }

}