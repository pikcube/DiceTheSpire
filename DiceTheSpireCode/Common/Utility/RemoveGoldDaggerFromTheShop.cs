using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Inventor.Rare;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Utility;

public class RemoveGoldDaggerFromTheShop() : CustomSingletonModel(HookType.Run)
{
    public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        return options.Where(c => c is not GoldDagger);
    }
}