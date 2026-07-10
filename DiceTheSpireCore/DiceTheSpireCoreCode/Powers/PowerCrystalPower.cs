using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class PowerCrystalPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if(cardPlay.Card.Type != CardType.Power || cardPlay.Card.Owner.Creature is null || cardPlay is null || Owner is null)
        {
            return;
        }

        int xValue = cardPlay.Card.EnergyCost.GetAmountToSpend();

        PowerCrystalPower powerCrystalPower = this;
        await PowerCmd.Apply<StrengthPower>(choiceContext, powerCrystalPower.Owner, xValue*Amount, cardPlay.Card.Owner.Creature, null);
    }
}