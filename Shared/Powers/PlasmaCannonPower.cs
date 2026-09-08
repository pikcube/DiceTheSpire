using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.Shared.Powers;


public class PlasmaCannonPower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Unplayable), HoverTipFactory.FromKeyword(BlinkModel.Blink)];
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature != Owner || !card.Keywords.Contains(CardKeyword.Unplayable))
        {
            return;
        }
        int unplayableDrawn = CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>()
            .Count(cde => cde.HappenedThisTurn(CombatState) && cde.Card.Owner.Creature == Owner && cde.Card.Keywords.Contains(CardKeyword.Unplayable));

        if (unplayableDrawn > Amount)
        {
            return;
        }

        await card.BlinkAsync(choiceContext);
        await CardPileCmd.Draw(choiceContext, card.Owner);
    }
}