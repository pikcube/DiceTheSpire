using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IOnShockListener
{
    public Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, CardModel card);
}