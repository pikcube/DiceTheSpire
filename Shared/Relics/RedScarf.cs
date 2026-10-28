using DiceTheSpire.Shared.Powers;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class RedScarf : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    private bool _usedThisCombat;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryPower>(1M)];
    public bool UsedThisCombat
    {
        get => this._usedThisCombat;
        private set
        {
            if (this._usedThisCombat == value)
                return;
            this.AssertMutable();
            this._usedThisCombat = value;
        }
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress || target != Owner.Creature || result.UnblockedDamage <= 0 || UsedThisCombat)
            return;
        Flash();
        UsedThisCombat = true;

        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, null);
    }

}