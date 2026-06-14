using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Powers;


public class CalculatorPower : TheInventorPower, IOnInspectListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task OnInspectAsync(PlayerChoiceContext choiceContext, int cards, CardModel[] selectedCards, Player inspector)
    {
        if (inspector != Owner.Player)
        {
            return;
        }

        await PlayerCmd.GainEnergy(1, inspector);
    }
}