using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;

public class PillowFort() : TheInventorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override string GetScrapId => nameof(Protection);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;


    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ReducePower>(3)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
    [
        HoverTipFactory.FromPower<ReducePower>(DynamicVars.Power<ReducePower>().IntValue)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await ReducePower.ApplyAsync(choiceContext, cardPlay.Target, DynamicVars.Power<ReducePower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ReducePower>().UpgradeValueBy(2);
    }
}