using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Countdown = DiceTheSpireCore.DiceTheSpireCoreCode.Extensions.Countdown;

namespace TheThief.TheThiefCode.Cards.Ancient;

  
public class MasterKey() : TheThiefCard(-1, CardType.Skill, CardRarity.Ancient, TargetType.Self), ICountdown
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new IntVar(nameof(CurrentCount), 3), new IntVar("PipCount", 2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>()];

    public async Task OnCountdownZero(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await Pip.CreateInHandAsync(Owner, (int)DynamicVars["PipCount"].BaseValue, CombatState);
        await Cmd.Wait(0.1f);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Countdown.OnCountdownPlay(choiceContext, cardPlay, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars["PipCount"].UpgradeValueBy(1);
    }
}