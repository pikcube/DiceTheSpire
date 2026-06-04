using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace TheThief.TheThiefCode.Powers;

  
public class BouncePower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay,
        ResourceInfo resources, PileType pileType, CardPilePosition position)
    {
        if (card.Owner.Creature != Owner ||
            CombatManager.Instance.History.CardPlaysStarted.Count(e =>
                    e.Actor == Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(CombatState)) >=
            Amount || pileType != PileType.Discard)
        {
            return (pileType, position);
        }
        return (PileType.Hand, CardPilePosition.Bottom);
    }

    public override Task AfterModifyingCardPlayResultPileOrPosition(CardModel card, PileType pileType, CardPilePosition position)
    {
        Flash();
        return Task.CompletedTask;
    }
}