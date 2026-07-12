using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;

public class BattleCry() : TheWarriorCard(3, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies), IRangeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VigorPower>(8M), new PowerVar<VulnerablePower>(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [BetterStaticHoverTips.RangeHoverTip(this), HoverTipFactory.FromPower<VigorPower>(DynamicVars.Power<VigorPower>().IntValue), HoverTipFactory.FromPower<VulnerablePower>(DynamicVars.Power<VulnerablePower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (RunState is null || CombatState is null)
        {
            return;
        }
        await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.Enemies, DynamicVars.Vulnerable.IntValue, Owner.Creature, cardPlay.Card);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars.Power<VigorPower>().IntValue, Owner.Creature, cardPlay.Card);
    }

    public int MinimumCost => 0;
    public int MaximumCost => 2;
    protected override void OnUpgrade()
    {
        DynamicVars.Power<VigorPower>().UpgradeValueBy(2);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }

}