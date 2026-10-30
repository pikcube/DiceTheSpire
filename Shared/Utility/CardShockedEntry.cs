using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Utility;

public class CardShockedEntry(CardModel card, ICombatState combatState, CombatHistory history)
    : CombatHistoryEntry(card.Owner.Creature, combatState.RoundNumber, combatState.CurrentSide, history,
        combatState.Players)
{
    public CardModel Card { get; } = card;

    public override string Description => $"{Actor.Player?.Character.Id.Entry} exhausted {Card.Id.Entry}";
}