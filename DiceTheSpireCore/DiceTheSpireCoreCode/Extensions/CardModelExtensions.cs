using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

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

        public Task NudgeAsync(PlayerChoiceContext choiceContext)
        {
            if (instance.CurrentUpgradeLevel > 0)
            {
                CardCmd.Downgrade(instance);
                return Task.CompletedTask;
            }

            return instance.ExhaustAsync(choiceContext);
        }
    }
}