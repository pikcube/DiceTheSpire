using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheThief.TheThiefCode.Powers;

namespace TheThief.TheThiefCode.Cards.Rare;

public class SnakeEyeCharm() : TheThiefCard(-1, CardType.Skill, CardRarity.Rare, TargetType.Self), ICountdown
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
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(CurrentCount), 3), new PowerVar<SnakeEyesPower>(2M), new EnergyVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CountdownModel.Countdown];

    public async Task OnCountdownZero(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        await PowerCmd.Apply<SnakeEyesPower>(choiceContext, Owner.Creature, DynamicVars["SnakeEyesPower"].BaseValue, Owner.Creature, this);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Countdown.OnCountdownPlay(choiceContext, cardPlay, this);
    }

    protected override void OnUpgrade()
    {
        this.UpgradeCountdown(-1);
    }
}