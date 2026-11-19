using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Powers;

public class CamouflagePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || Owner.Player.Character.CardPool.AllCards.Contains(cardPlay.Card))
        {
            return;
        }

        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, cardPlay);
    }
}