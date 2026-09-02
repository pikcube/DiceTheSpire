using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Cards.Rare;

public class GoldDagger() : TheInventorCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), IScrapCard
{
    public override string GetScrapId => nameof(Harvest);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, DamageProps.card)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];
    public override IEnumerable<CardTag> Tags => [CardTag.Strike];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    public bool IsAlwaysOfferedAsScrap => IsUpgraded;
}