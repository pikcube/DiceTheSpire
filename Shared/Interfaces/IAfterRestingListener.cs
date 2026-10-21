using MegaCrit.Sts2.Core.Entities.Players;

namespace DiceTheSpire.Shared.Interfaces;

public interface IAfterRestingListener
{
    public Task BetterAfterRestSiteHealAsync(Player player);
}