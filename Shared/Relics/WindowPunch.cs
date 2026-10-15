using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Relics;

public class WindowPunch : TheThiefRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner != Owner || card is not ICountdown count)
        {
            return;
        }

        if (CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count(e =>
                e.HappenedThisTurn(Owner.Creature.CombatState) && e.Card.Owner == Owner && e.Card is ICountdown) > 1)
        {
            return;
        }

        count.AddCountThisTurnOrUntilPlayed(-1);
    }
}