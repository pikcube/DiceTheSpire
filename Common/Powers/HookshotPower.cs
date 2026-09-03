using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Powers;

  
public class HookshotPower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card.Owner.Creature != Owner || cardLocation.pileType != PileType.Discard)
        {
            return cardLocation;
        }
        return new CardLocation(card.Owner, PileType.Hand, CardPilePosition.Bottom);
    }

    public override async Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation cardLocation)
    {
        Flash();
        await PowerCmd.Decrement(this);
    }
}