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
        public async Task BumpAsync(PlayerChoiceContext choiceContext)
        {
            if (instance.IsUpgradable)
            {
                CardCmd.Upgrade(instance);
                await DiceyHooks.OnAfterBumpAsync(choiceContext, instance, null);
            }
            else
            {
                CardModel newCard = instance.CreateClone();
                newCard.DowngradeInternal();
                await CardPileCmd.AddGeneratedCardToCombat(newCard, PileType.Hand, instance.Owner);
                await DiceyHooks.OnAfterBumpAsync(choiceContext, instance, newCard);
            }

            
        }

        public async Task NudgeAsync(PlayerChoiceContext choiceContext)
        {
            if (instance.CurrentUpgradeLevel <= 0)
            {
                await instance.ExhaustAsync(choiceContext, true);
                await DiceyHooks.OnAfterNudgeAsync(choiceContext, instance, true);
            }
            else
            {
                CardCmd.Downgrade(instance);
                await DiceyHooks.OnAfterNudgeAsync(choiceContext, instance, false);
            }
        }
    }
}