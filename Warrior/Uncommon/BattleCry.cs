using BaseLib.Extensions;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Uncommon;

public class BattleCry() : TheWarriorCard(3, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies), IFuryModifier
{
    public bool ShouldIgnoreFury => true;
    public bool ShouldMaintainFury => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryPower>(1M), new PowerVar<VulnerablePower>(2), .. RangeVars.Make(0, 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (RunState is null || CombatState is null)
        {
            return;
        }
        await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.Enemies, DynamicVars.Vulnerable.IntValue, Owner.Creature, cardPlay.Card);
        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, DynamicVars.Power<FuryPower>().IntValue, Owner.Creature, cardPlay.Card);
    }
    protected override void OnUpgrade()
    {
        //DynamicVars.Vulnerable.UpgradeValueBy(1);
        DynamicVars.MaxRange.UpgradeValueBy(-2);
    }

}