using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheThief.TheThiefCode.Cards.Common;

  
public class PoisonNeedle() : TheThiefCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(4)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, DynamicVars.Poison.BaseValue,
            Owner.Creature, this);
        await Cmd.Wait(0.2f);
    }

    //todo: This one is a bit more complicated to rewrite, best of luck Cory
    protected override (PileType, CardPilePosition) GetResultPileTypeAndPositionForCardPlay()
    {
        throw new NotImplementedException();
    }

    //protected override PileType GetResultPileTypeForCardPlay()
    //{
    //    return base.GetResultPileTypeForCardPlay() != PileType.Discard ? base.GetResultPileTypeForCardPlay() : PileType.Hand;
    //}

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(2);
    }
}