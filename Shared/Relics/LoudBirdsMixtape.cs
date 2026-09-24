using DiceTheSpire.Shared.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Relics;

public class LoudBirdsMixtape : TheThiefRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1)];

    private List<PowerModel> _triggeredPowers = [];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (applier != Owner.Creature || power.Type != PowerType.Debuff || _triggeredPowers.Contains(power.CanonicalInstance))
        {
            return;
        }

        _triggeredPowers.Add(power.CanonicalInstance);
        
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue,
            Owner.Creature, null);
    }

    public override async Task BeforeCombatStart()
    {
        _triggeredPowers.Clear();
    }
}