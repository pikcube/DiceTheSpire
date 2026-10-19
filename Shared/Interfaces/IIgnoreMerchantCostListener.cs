using MegaCrit.Sts2.Core.Entities.Merchant;

namespace DiceTheSpire.Shared.Utility;

public static partial class DiceyHooks
{
    public interface IIgnoreMerchantCostListener
    {
        public bool TryIgnoreCost(MerchantEntry entry, MerchantInventory? inventory);

        public Task AfterCostIgnored(MerchantEntry entry, MerchantInventory? inventory, bool wasPurchaseSuccessful);
    }
}