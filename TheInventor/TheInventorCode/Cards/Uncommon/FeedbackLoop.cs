using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class FeedbackLoop() : TheInventorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(MagicDice);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new EnergyVar(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await FeedbackLoopPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Cards.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}