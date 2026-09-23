using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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

    private List<PowerModel> triggeredPowers = new List<PowerModel>();

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (applier != Owner.Creature || power.Type != PowerType.Debuff) //Add check for if power is in list of triggered debuffs
        {
            return;
        }

        if (CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>().Any(entries =>
                entries.Applier == Owner.Creature && entries.Power.Type == PowerType.Debuff &&
                entries.Power.GetType() == power.GetType()))
        {
            return;
        }

        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue,
            Owner.Creature, null);
    }

    public override Task BeforeCombatStart()
    {
        //clear list of triggered debuffs

        return base.BeforeCombatStart();
    }
}