using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Commands;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.DiceTheSpireCode.Warrior.Uncommon;
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
