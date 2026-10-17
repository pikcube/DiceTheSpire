using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Warrior.Uncommon;

public class CrystalShield() : TheWarriorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self), ICrystalCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedBlock(0, Bonus)];
    public override bool GainsBlock => true;
    private static decimal Bonus(CardModel card, Creature? arg2)
    {
        if (card.IsUpgraded)
        {
            return card.Owner.PlayerCombatState is null ? 0 : card.Owner.PlayerCombatState.Hand.Cards.Sum(c => c.EnergyCost.GetAmountToSpend()) * (3);
        }
        else
        {
            return card.Owner.PlayerCombatState is null ? 0 : card.Owner.PlayerCombatState.Hand.Cards.Sum(c => c.EnergyCost.GetAmountToSpend()) * (2);
        }
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CrystalShield crystalShield = this;
        await CreatureCmd.GainBlock(crystalShield.Owner.Creature, crystalShield.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target) + (crystalShield.EnergyCost.GetAmountToSpend() * 2), crystalShield.DynamicVars.CalculatedBlock.Props, cardPlay);
    }

}