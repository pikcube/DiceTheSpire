using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.DiceTheSpireCode.Thief.Uncommon;

public class Rat() : TheThiefCard(-1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy), ICountdown
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new PowerVar<PoisonPower>(5), new IntVar("CurrentCount", 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, DynamicVars.Poison.BaseValue, Owner.Creature,
            this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.EnchantedValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(1);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}