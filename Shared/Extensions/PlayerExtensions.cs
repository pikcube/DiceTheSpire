using DiceTheSpire.Shared.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Shared.Extensions;

public static class PlayerExtensions
{
    extension(Player instance)
    {
        public Task<int> InspectAsync(PlayerChoiceContext choiceContext, int cards)
        {
            return InspectCmd.InspectAsync(choiceContext, instance, cards);
        }
    }
}