using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;


[UsedImplicitly]
public class StartledPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Debuff;

    public override bool AllowNegative => false;

    public override PowerStackType StackType => PowerStackType.Counter;

    public bool IsTurnEnding { get; set; }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Decrement(this);
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
        }
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        if (IsTurnEnding || oldOwner.Player is null)
        {
            return Task.CompletedTask;
        }

        IsTurnEnding = true;
        PlayerCmd.EndTurn(oldOwner.Player, false);
        return Task.CompletedTask;
    }

    public override Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        IsTurnEnding = true;
        return PowerCmd.Remove(this);
    }
}