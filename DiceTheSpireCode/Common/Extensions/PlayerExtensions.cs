using DiceTheSpire.DiceTheSpireCode.Common.Keywords;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.DiceTheSpireCode.Common.Extensions;

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