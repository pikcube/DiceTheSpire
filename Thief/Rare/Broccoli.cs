using BaseLib.Extensions;
using DiceTheSpire.Common.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Rare;

public class Broccoli() : TheThiefCard(-1, CardType.Power, CardRarity.Rare, TargetType.Self), ICountdown
{
    public int MaxCount
    {
        get;
        set
        {
            int changeBy = value - field;
            field = value;
            CurrentCount += changeBy;
        }
    } = 4;

    public int CurrentCount
    {
        get => DynamicVars[nameof(CurrentCount)].IntValue;
        set
        {
            DynamicVars[nameof(CurrentCount)].BaseValue = value;
            if (DynamicVars[nameof(CurrentCount)].BaseValue < 0)
            {
                DynamicVars[nameof(CurrentCount)].BaseValue = 0;
            }

            if (DynamicVars[nameof(CurrentCount)].BaseValue > MaxCount)
            {
                DynamicVars[nameof(CurrentCount)].BaseValue = MaxCount;
            }
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<DexterityPower>(1), new PowerVar<ThornsPower>(3), new IntVar(nameof(CurrentCount), 4)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars.Dexterity.BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars.Power<ThornsPower>().BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ThornsPower>().UpgradeValueBy(2);
    }
}