using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Common;

public class BearRoar() : TheThiefCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies), ICountdown
{
    public int BaseCount { get; set; } = 3;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.HittableEnemies, DynamicVars.Weak.BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.UpgradeCountdownBy(-1);
    }
}