using BaseLib.Abstracts;
using DiceTheSpire.Shared.Interfaces;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace DiceTheSpire.Shared.Utility;

//Normally CustomSingltons live in the core, but this one should only be touched by the Countdown patch, so it lives in the patch layer.
[UsedImplicitly]
public class CountdownCanceller() : CustomSingletonModel(HookType.Combat)
{
    //Do not touch. Responsible for keeping track of which cards needs to be cancelled.
    private static List<ICountdown> Cancelled { get; set; } = [];

    //Ensure we always keep this list empty.
    public override Task BeforeRoomEntered(AbstractRoom room)
    {
        Cancelled = [];
        return Task.CompletedTask;
    }

    //Ensure we always keep this list empty.
    public override Task AfterCombatVictory(CombatRoom room)
    {
        Cancelled = [];
        return Task.CompletedTask;
    }

    //Queues a card for cancellation.
    //This call is not idempotent, calling this more than once will cancel multiple card plays for this card, regardless of the current countdown status.
    public static void Cancel(ICountdown countdown)
    {
        Cancelled.Add(countdown);
    }

    //Set the playcount to 0 if the countdown has been cancelled
    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card is not ICountdown c || !IsCancelled(c))
        {
            return playCount;
        }

        Cancelled.Remove(c);
        return 0;

    }

    //In the event a card play is cancelled, we must manually move it out of the play queue
    //Otherwise the game breaks.
    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        await CardPileCmd.Add(card, PileType.Discard);
    }

    //Check if the current countdown has has its cardplay cancelled
    public static bool IsCancelled(ICountdown countdown)
    {
        return Cancelled.Contains(countdown);
    }
}