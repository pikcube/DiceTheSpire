using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Shared.Powers;

public class LuckyDrawPower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.EnergyCost.GetResolved() != 1)
        {
            return;
        }
        await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
    }
}