using DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class PlayerExtensions
{
    extension(Player instance)
    {
        public Task<int> InspectAsync(PlayerChoiceContext choiceContext, int cards)
        {
            return InspectModel.InspectAsync(choiceContext, instance, cards);
        }
    }
}