using DiceTheSpire.Common.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Extensions;

public static class CardModelExtensions
{
    extension<T>(T instance) where T : CardModel
    {
        public async Task BumpAsync(PlayerChoiceContext choiceContext, PileType? destinationPile = null)
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
                PileType newPileType = destinationPile ?? instance.Pile?.Type ?? PileType.Hand;
                if (newPileType is PileType.Draw)
                {
                    await CardPileCmd.AddGeneratedCardToCombat(newCard, PileType.Draw, instance.Owner, CardPilePosition.Random);
                }
                else
                {
                    await CardPileCmd.AddGeneratedCardToCombat(newCard, newPileType, instance.Owner);
                }
                await DiceyHooks.OnAfterBumpAsync(choiceContext, instance, newCard);
            }

            
        }

        //public async Task NudgeAsync(PlayerChoiceContext choiceContext)
        //{
        //    if (instance.CurrentUpgradeLevel <= 0)
        //    {
        //        await instance.ExhaustAsync(choiceContext, true);
        //        await DiceyHooks.OnAfterNudgeAsync(choiceContext, instance, true);
        //    }
        //    else
        //    {
        //        CardCmd.Downgrade(instance);
        //        await DiceyHooks.OnAfterNudgeAsync(choiceContext, instance, false);
        //    }
        //}

    }
}