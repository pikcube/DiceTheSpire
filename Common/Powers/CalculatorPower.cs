using DiceTheSpire.Common.Listeners;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Powers;


public class CalculatorPower : TheInventorPower, IAfterInspectListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private int Counter { get; set; }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == Owner.Side)
        {
            Counter = 0;
        }

        return Task.CompletedTask;
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        Counter = -1;
        return Task.CompletedTask;
    }

    public async Task AfterInspectAsync(PlayerChoiceContext choiceContext, int cards, CardModel[] selectedCards, Player inspector)
    {
        if (inspector != Owner.Player || Counter < 0 || Counter >= Amount)
        {
            return;
        }

        await CardPileCmd.Draw(choiceContext, inspector);
        ++Counter;
        if (Counter >= Amount)
        {
            Counter = -1;
        }
    }
}