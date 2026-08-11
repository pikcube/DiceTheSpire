using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;

public interface IAfterInspectListener
{
    public Task AfterInspectAsync(PlayerChoiceContext choiceContext, int cards, CardModel[] selectedCards, Player inspector);
}