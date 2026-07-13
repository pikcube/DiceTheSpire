using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Entities.Cards;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.HoverTips;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;



namespace TheWarrior.TheWarriorCode.Cards.Rare;
public class Shriek() : TheWarriorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self), IRangeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WarriorShriekPower>(1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [BetterStaticHoverTips.RangeHoverTip(this), HoverTipFactory.FromPower<VigorPower>(DynamicVars.Power<VigorPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WarriorShriekPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    public int MinimumCost => 1;
    public int MaximumCost => 3;
}

