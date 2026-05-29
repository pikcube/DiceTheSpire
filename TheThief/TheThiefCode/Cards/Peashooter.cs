using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheThief.TheThiefCode.Cards;

public class Peashooter() : TheThiefCard(-1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy), ICountdown
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

    public int CurrentCount { 
        get;
        set
        {
            field = value;
            if (field < 0)
            {
                field = 0;
            }

            if (field > MaxCount)
            {
                field = MaxCount;
            }
        }
    } = 2;

    public bool WasCountdownJustFinished
    {
        get
        {
            bool b = field;
            field = false;
            return b;
        }
        set;
    }

    public async Task OnCountdownZero(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        WasCountdownJustFinished = true;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4M, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CountdownModel.Countdown];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Countdown.OnCountdownPlay(choiceContext, cardPlay, this);
    }

    protected override PileType GetResultPileTypeForCardPlay()
    {
        if (WasCountdownJustFinished)
        {
            return PileType.Hand;
        }
        else
        {
            return PileType.Discard;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
    }
}