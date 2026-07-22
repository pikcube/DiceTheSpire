using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;

public class HealingCrystal() : TheWarriorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3), new HealVar(7M)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState is null)
        {
            return;
        }
        if (IsPlayable)
        {
            await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.IntValue);
        }
    }
    protected override bool IsPlayable => Owner.PlayerCombatState?.Hand.Cards.Sum(c => c.EnergyCost.GetAmountToSpend()) <= DynamicVars.Energy.IntValue;
    protected override bool ShouldGlowGoldInternal => IsPlayable;
    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2);
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}