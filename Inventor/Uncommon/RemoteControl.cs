using DiceTheSpire.Common.Utility;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Uncommon;


public class RemoteControl() : TheInventorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(Efficiency);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, DamageProps.card)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await ScrapManager.RandomizeAllGadgetsAsync(choiceContext, Owner, cardPlay.Card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}