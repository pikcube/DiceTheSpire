using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Powers;

  
public class BouncePower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card.Owner.Creature != Owner ||
            CombatManager.Instance.History.CardPlaysStarted.Count(e =>
                e.Actor == Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(CombatState)) >=
            Amount || cardLocation.pileType != PileType.Discard)
        {
            return cardLocation;
        }
        return new CardLocation(cardLocation.player, PileType.Hand, CardPilePosition.Bottom);
    }

    public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation cardLocation)
    {
        Flash();
        return Task.CompletedTask;
    }
}