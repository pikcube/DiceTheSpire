using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Commands;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;
public class Tazer() : TheWarriorCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(10), new PowerVar<ShockPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Rummage), HoverTipFactory.FromPower<ShockPower>(DynamicVars.Power<ShockPower>().IntValue)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ShockPower>(choiceContext, Owner.Creature, DynamicVars.Power<ShockPower>().IntValue, Owner.Creature, this);

        await RummageCmd.RummageAsync(choiceContext, Owner, DynamicVars.Cards.IntValue, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ShockPower>().UpgradeValueBy(-1);
    }
}
