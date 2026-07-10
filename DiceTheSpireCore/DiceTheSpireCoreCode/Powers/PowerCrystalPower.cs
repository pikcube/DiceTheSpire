using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class PowerCrystalPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        if (cardPlay.Card.Type != CardType.Power)
        {
            return;
        }

        int xValue = cardPlay.Card.EnergyCost.GetAmountToSpend();

        PowerCrystalPower powerCrystalPower = this;
        await PowerCmd.Apply<StrengthPower>(choiceContext, powerCrystalPower.Owner, xValue*Amount, cardPlay.Card.Owner.Creature, null);
    }
}