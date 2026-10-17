using BaseLib.Extensions;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Uncommon;

public class Tazer() : TheWarriorCard(3, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(3), new PowerVar<WeakPower>(2), .. RangeVars.Make(0, 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(DynamicVars.Power<WeakPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        if (RunState is null || CombatState is null)
        {
            return;
        }
        //await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.Enemies, DynamicVars.Weak.IntValue, Owner.Creature, cardPlay.Card);
        for (int n = 0; n < DynamicVars.Repeat.IntValue; ++n)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars.Weak.IntValue, Owner.Creature, cardPlay.Card);
        }
    }
    protected override void OnUpgrade()
    {
        //DynamicVars.Weak.UpgradeValueBy(1);
        DynamicVars.MaxRange.UpgradeValueBy(-2);
    }

}