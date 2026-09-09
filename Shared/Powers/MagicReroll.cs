using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class MagicRerollPower : DiceTheSpireCorePower
{

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player is null || Owner.Player.PlayerCombatState is null)
        {
            return;
        }

        List<CardModel> allCards = [.. Owner.Player.PlayerCombatState.AllCards];
        foreach (CardModel c in allCards)
        {
            if (c is RollAgain)
            {
                CardCmd.Upgrade(c);
            }
        }
    }

    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (card is RollAgain && creator is not null && creator == Owner.Player)
        {
            CardCmd.Upgrade(card);
        }

        return Task.CompletedTask;
    }
}

