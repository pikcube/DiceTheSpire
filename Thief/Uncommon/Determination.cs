using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Uncommon;

public class Determination() : TheThiefCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self), ICountdown
{
    public int BaseCount { get; set; } = 2;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DeterminationPower>(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<DeterminationPower>(choiceContext, Owner.Creature,
            DynamicVars.Power<DeterminationPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}