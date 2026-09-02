using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Cards.Rare;

namespace TheInventor.TheInventorCode.Utilities;

public class RemoveGoldDaggerFromTheShop() : CustomSingletonModel(HookType.Run)
{
    public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        return options.Where(c => c is not GoldDagger);
    }
}