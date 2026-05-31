using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
        if (card.Owner.Creature != this.Owner ||
            CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>(
                (Func<CardPlayStartedEntry, bool>)(e =>
                    e.Actor == this.Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(this.CombatState))) >=
            this.Amount || pileType != PileType.Discard)
        {
            return (pileType, position);
        }
        return (PileType.Hand, CardPilePosition.Bottom);
    }

    public override Task AfterModifyingCardPlayResultPileOrPosition(CardModel card, PileType pileType, CardPilePosition position)
    {
        this.Flash();
        return Task.CompletedTask;
    }
}