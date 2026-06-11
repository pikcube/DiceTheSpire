using DiceTheSpireCore.DiceTheSpireCoreCode;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Common;


public class HardHat() : TheInventorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override string GetScrapId => nameof(Protection);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new BlockVar(8, BlockProps.card)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Inspect, DynamicVars.Cards)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await Owner.InspectAsync(choiceContext, DynamicVars.Cards.IntValue);

    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}