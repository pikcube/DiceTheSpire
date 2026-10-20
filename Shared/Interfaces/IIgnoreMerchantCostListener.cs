using MegaCrit.Sts2.Core.Entities.Merchant;

namespace DiceTheSpire.Shared.Interfaces;

public interface IIgnoreMerchantCostListener
{
    public bool TryIgnoreCost(MerchantEntry entry, MerchantInventory? inventory);

    public Task AfterCostIgnored(MerchantEntry entry, MerchantInventory? inventory, bool wasPurchaseSuccessful);
}