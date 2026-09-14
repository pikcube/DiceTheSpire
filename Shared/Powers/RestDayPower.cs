using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class RestDayPower : DiceTheSpireCorePower, IAfterRerollListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public async Task AfterRerollAsync(PlayerChoiceContext choiceContext, CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
    {
        CardCmd.ApplyKeyword(card, CardKeyword.Retain);
    }

}
