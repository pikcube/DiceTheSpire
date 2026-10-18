using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.WorkoutQuests;
public class StunlockQuest : WorkoutQuestCard, IInterruptRemovalListener
{
    //Remove this card
    public override CardModel RewardCard => Stunlock.Create();

    public bool TryInterruptRemoval(CardModel card)
    {
        return card == this;
    }

    public async Task AfterRemovalInterrupted(CardModel card)
    {
        if (card == this)
        {
            await OnWorkoutComplete();
        }
    }
}