using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class RoseShield() : TheInventorCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Needle);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(11, BlockProps.card), new PowerVar<ThornsPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block), HoverTipFactory.FromPower<ThornsPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await RoseShieldPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<ThornsPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4);
        DynamicVars.Power<ThornsPower>().UpgradeValueBy(1);
    }
}