using DiceTheSpire.Common.Interfaces;
using DiceTheSpire.Common.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Rare;

public class Camouflage() : TheThiefCard(-1, CardType.Power, CardRarity.Rare, TargetType.Self), ICountdown
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ReducePower>(3), new IntVar(nameof(CurrentCount), 4)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ReducePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<ReducePower>(choiceContext, Owner.Creature, DynamicVars["ReducePower"].BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ReducePower"].UpgradeValueBy(1);
    }
}