using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;

public class Grindstone() : TheInventorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(PowerUp);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<StrengthPower>(3), new PowerVar<GrindstonePower>(50)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<GrindstonePower>(DynamicVars.Power<GrindstonePower>().IntValue)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await StrengthPower.ApplyAsync(choiceContext, Owner.Creature, -DynamicVars.Strength.IntValue,
            Owner.Creature, this);
        await GrindstonePower.ApplyAsync(choiceContext, Owner.Creature,
            DynamicVars.Power<GrindstonePower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(-2);
    }
}