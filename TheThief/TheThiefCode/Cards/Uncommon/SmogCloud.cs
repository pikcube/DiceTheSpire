using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class SmogCloud() : TheThiefCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(8), new PowerVar<WeakPower>(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<PoisonPower>(choiceContext, CombatState.Enemies, DynamicVars.Poison.BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.Enemies, DynamicVars.Weak.BaseValue, Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(3);
        DynamicVars.Weak.UpgradeValueBy(1);
    }
}