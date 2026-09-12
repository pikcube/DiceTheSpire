using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Extensions;

public static class CardModelExtensions
{
    extension<T>(T instance) where T : CardModel
    {
        public bool ShouldShockOnNextPlay
        {
            get => ShockModel.ShouldShock(instance);
            set => ShockModel.SetShouldShock(instance, value);
        }

        public async Task BumpAsync(PlayerChoiceContext choiceContext, PileType? destinationPile = null)
        {
            if (instance.IsUpgradable)
            {
                CardCmd.Upgrade(instance);
                await DiceyHooks.OnAfterBumpAsync(choiceContext, instance, null);
            }
            else
            {
                T newCard = instance.CreateNewInstance(instance.Owner, instance.CombatState);
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

        public T CreateNewInstance(Player owner, ICombatState? combatState = null)
        {
            return combatState is null
                ? owner.RunState.CreateCard<T>(owner)
                : combatState.CreateCard<T>(owner);
        }

        public Task ShockAsync(PlayerChoiceContext choiceContext, bool skipVisuals = false) => ShockModel.ShockCardAsync(choiceContext, instance, skipVisuals);

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