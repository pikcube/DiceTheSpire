using BaseLib.Utils;
using DiceTheSpire.Common.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Token;

[Pool(typeof(TokenCardPool))]
public class RollTheBones() : TheThiefCard(-1, CardType.Skill, CardRarity.Token, TargetType.AnyEnemy), ICountdown
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

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar(nameof(CurrentCount), 3), new SummonVar(5), new PowerVar<DoomPower>(8)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon), HoverTipFactory.FromPower<DoomPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, DynamicVars.Doom.BaseValue, Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Summon.UpgradeValueBy(2);
        DynamicVars.Doom.UpgradeValueBy(4);
    }
}