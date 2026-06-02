using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheThief.TheThiefCode.Powers;

  public class SnakeEyesPower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card = cardPlay.Card;
        if (Owner.Player != card.Owner || card.EnergyCost.Canonical != 1M)
        {
            return;
        }
        await Cmd.Wait(0.2f);
        await PlayerCmd.GainEnergy(1M, Owner.Player);
        await PowerCmd.Decrement(this);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner.Side == side && Owner.HasPower<SnakeEyesPower>())
        {
            await PowerCmd.Remove(this);
        }
    }
}