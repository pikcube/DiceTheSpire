using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Warrior.Common;


public class Crunch() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new DamageVar(7M, DamageProps.card)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        if (IsUpgraded)
        {
            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToModifyCost, 0, DynamicVars.Cards.IntValue);
            CardModel[] cardChoices = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
            foreach (CardModel card in cardChoices)
            {
                if (!card.EnergyCost.CostsX)
                {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(card.EnergyCost.GetAmountToSpend() - 1);
                }
            }
        }
        else
        {
            CardModel[] cards =
            [
                ..PileType.Hand.GetPile(Owner).Cards
                    .Where(c => !c.Keywords.Contains(CardKeyword.Unplayable) && !c.EnergyCost.CostsX)
                    .TakeRandom((int)DynamicVars.Cards.BaseValue, Owner.RunState.Rng.CombatCardSelection)
            ];
            foreach (CardModel card in cards)
            {

                card.EnergyCost.SetThisTurnOrUntilPlayed(card.EnergyCost.GetAmountToSpend() - 1);
            }
        }
    }
}