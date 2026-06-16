using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class CardModelExtensions
{
    extension<T>(T instance) where T : CardModel
    {
        public async Task BumpAsync()
        {
            if (instance.IsUpgradable)
            {
                CardCmd.Upgrade(instance);
                return;
            }

            CardModel newCard = instance.CreateClone();
            newCard.DowngradeInternal();
            await CardPileCmd.AddGeneratedCardToCombat(newCard, PileType.Hand, instance.Owner);
        }

        public async Task NudgeAsync(PlayerChoiceContext choiceContext)
        {
            if (instance.CurrentUpgradeLevel <= 0)
            {
                await instance.ExhaustAsync(choiceContext);
                return;
            }

            CardCmd.Downgrade(instance);
        }
    }
}