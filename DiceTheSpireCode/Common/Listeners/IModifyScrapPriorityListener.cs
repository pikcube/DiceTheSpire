using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Listeners;

public interface IModifyScrapPriorityListener
{
    public void ModifyPriority(Player player, ref List<CardModel> scrapCards, ref List<CardModel> otherCards);
}