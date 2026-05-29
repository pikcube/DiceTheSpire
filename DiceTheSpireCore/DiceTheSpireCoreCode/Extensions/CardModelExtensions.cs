using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class CardModelExtensions
{
    extension<T>(T instance) where T : CardModel
    {
        public Task BumpAsync()
        {
            if (instance.IsUpgradable)
            {
                CardCmd.Upgrade(instance);
                return Task.CompletedTask;
            }

            CardModel newCard = instance.CreateClone();
            newCard.DowngradeInternal();
            return CardPileCmd.AddGeneratedCardToCombat(newCard, PileType.Hand, instance.Owner);
        }
    }
}