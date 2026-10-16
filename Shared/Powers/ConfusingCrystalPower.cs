using DiceTheSpire.Shared.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;
public class ConfusingCrystalPower : DiceTheSpirePower
{

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Player is null)
        {
            return;
        }
        foreach (CardModel card in PileType.Hand.GetPile(Owner.Player).Cards.Where(c => !c.EnergyCost.CostsX))
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
            {
                await RerollCmd.RerollAsync(choiceContext, card, RerollDuration.UntilEndOfTurnOrPlayed);
            }
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
    IEnumerable<Creature> owner)
    {
        if (side == Owner.Side)
        {
            return;
        }
        
        await PowerCmd.Decrement(this);
        
        Flash();

    }

}