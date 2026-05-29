using System.Dynamic;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheThief.TheThiefCode.Cards;

  //todo: make countdown
public class Peashooter() : TheThiefCard(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy), ICountdown
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4M, ValueProp.Move)];



    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override PileType GetResultPileTypeForCardPlay()
    {
        PileType pileTypeForCardPlay = base.GetResultPileTypeForCardPlay();
        return pileTypeForCardPlay != PileType.Discard ? pileTypeForCardPlay : PileType.Hand;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
    }
}