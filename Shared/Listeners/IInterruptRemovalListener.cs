using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IInterruptRemovalListener
{
    public bool TryInterruptRemoval(CardModel card);
    public Task AfterRemovalInterrupted(CardModel card);
}