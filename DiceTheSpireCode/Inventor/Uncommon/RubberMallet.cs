using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;

public class RubberMallet() : TheInventorCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(11, DamageProps.card), new EnergyVar(2), new CardsVar(2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];

    public override string GetScrapId => nameof(Efficiency);

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card == this && Owner.PlayerCombatState is not null && Owner.PlayerCombatState.Energy < DynamicVars.Energy.IntValue)
        {
            modifiedCost = 0;
            return true;
        }
        modifiedCost = originalCost; 
        return false;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToBlink, DynamicVars.Cards.IntValue, DynamicVars.Cards.IntValue);
        IEnumerable<CardModel> results = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);
        await Task.WhenAll(results.Select(card => card.BlinkAsync(choiceContext)));

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
}