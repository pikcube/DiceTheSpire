using BaseLib.Extensions;
using DiceTheSpire.Common.Powers;
using DiceTheSpire.Inventor.Gadgets;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.Inventor.Rare;


[UsedImplicitly]
public class Resistor() : TheInventorCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Protection);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ReducePower>(9)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
    [
        HoverTipFactory.FromPower<ReducePower>(DynamicVars.Power<ReducePower>().IntValue), HoverTipFactory.FromKeyword(BlinkModel.Blink)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ResistorPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<ReducePower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ReducePower>().UpgradeValueBy(3);
    }
}