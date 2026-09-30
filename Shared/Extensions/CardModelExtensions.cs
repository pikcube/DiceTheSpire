using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
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

        public async Task BumpAsync(PlayerChoiceContext choiceContext)
        {
            await BumpCmd.BumpAsync(choiceContext, instance);
        }

        public Task ShockAsync(PlayerChoiceContext choiceContext, bool skipVisuals = false) => ShockModel.ShockCardAsync(choiceContext, instance, skipVisuals);
        public Task UnshockAsync(PileType destination = PileType.Hand) => ShockModel.UnshockCardAsync(instance, destination);
    }
}