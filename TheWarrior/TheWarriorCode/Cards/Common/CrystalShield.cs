using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheWarrior.TheWarriorCode.Cards.Common;

public class CrystalShield() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedBlock(0, Bonus)];

    //protected override HashSet<CardTag> CanonicalTags => [CardTag.Crystal];
    public override bool GainsBlock => true;
    private static decimal Bonus(CardModel card, Creature? arg2)
    {
        return card.Owner.PlayerCombatState is null ? 0 : card.Owner.PlayerCombatState.Hand.Cards.Sum(c => c.EnergyCost.GetAmountToSpend()) * 2;
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CrystalShield crystalShield = this;
        await CreatureCmd.GainBlock(crystalShield.Owner.Creature, crystalShield.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), crystalShield.DynamicVars.CalculatedBlock.Props, cardPlay);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}