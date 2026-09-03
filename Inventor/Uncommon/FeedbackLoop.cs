using BaseLib.Extensions;
using DiceTheSpire.Common.Powers;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.Inventor.Uncommon;

public class FeedbackLoop() : TheInventorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FeedbackLoopPower>(3)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink), HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FeedbackLoopPower>(choiceContext, Owner.Creature, DynamicVars.Power<FeedbackLoopPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<FeedbackLoopPower>().UpgradeValueBy(1);
    }

    public override string GetScrapId => nameof(Protection);
}