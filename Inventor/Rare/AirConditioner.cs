using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.Inventor.Rare;

public class AirConditioner() : TheInventorCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(WallOfIce);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(4, BlockProps.card), new RepeatVar(4)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int n = 0; n < DynamicVars.Repeat.IntValue; ++n)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.IntValue, BlockProps.card, cardPlay);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1);
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}