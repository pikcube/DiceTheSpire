using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheThief.TheThiefCode.Cards;

public class Cloak() : TheThiefCard(-1, CardType.Skill, CardRarity.Common, TargetType.Self), ICountdown
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
    } = 2;

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

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(nameof(CurrentCount), 2), new PowerVar<ReducePower>(1)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CountdownModel.Countdown];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ReducePower>()];

    public async Task OnCountdownZero(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ReducePower>(choiceContext, Owner.Creature, DynamicVars.Power<ReducePower>().BaseValue, Owner.Creature, this);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Countdown.OnCountdownPlay(choiceContext, cardPlay, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ReducePower>().UpgradeValueBy(1);
    }
}