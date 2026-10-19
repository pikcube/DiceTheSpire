using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;

namespace DiceTheSpire.Shared.Relics;

public class CounterfeitCoinPurse : TheThiefRelic, DiceyHooks.IIgnoreMerchantCostListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room.RoomType == RoomType.Shop)
        {
            Status = RelicStatus.Active;
        }
        return Task.CompletedTask;
    }

    public bool TryIgnoreCost(MerchantEntry entry, MerchantInventory? inventory)
    {
        if (inventory?.Player != Owner || Status != RelicStatus.Active)
        {
            return false;
        }

        return !entry.EnoughGold;
    }

    public Task AfterCostIgnored(MerchantEntry entry, MerchantInventory? inventory, bool wasPurchaseSuccessful)
    {
        if (wasPurchaseSuccessful)
        {
            Status = RelicStatus.Normal;
        }

        return Task.CompletedTask;
    }
}