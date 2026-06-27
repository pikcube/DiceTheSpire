using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheThief.TheThiefCode.Powers;

namespace TheThief.TheThiefCode.Cards.Ancient;

public class Upkeep() : TheThiefCard(2, CardType.Power, CardRarity.Ancient, TargetType.Self), ITomeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<UpkeepPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}