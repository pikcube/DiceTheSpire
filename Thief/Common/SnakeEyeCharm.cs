using DiceTheSpire.Common.Extensions;
using DiceTheSpire.Common.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Common;

public class SnakeEyeCharm() : TheThiefCard(-1, CardType.Skill, CardRarity.Common, TargetType.Self), ICountdown
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
    } = 3;

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
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(CurrentCount), 3), new EnergyVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        this.UpgradeCountdown(-1);
    }
}