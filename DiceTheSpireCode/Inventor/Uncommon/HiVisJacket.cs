using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpire.DiceTheSpireCode.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;

public class HiVisJacket() : TheInventorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<HiVisJacketPower>(3)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
    [
        HoverTipFactory.Static(BetterStaticHoverTips.Inspect,
            new CardsVar(DynamicVars.Power<HiVisJacketPower>().IntValue)), 
        HoverTipFactory.FromKeyword(BlinkModel.Blink)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await HiVisJacketPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<HiVisJacketPower>().IntValue, Owner.Creature, this);
    }

    public override string GetScrapId => nameof(Accelerate);

    protected override void OnUpgrade()
    {
        DynamicVars.Power<HiVisJacketPower>().UpgradeValueBy(1);
    }
}