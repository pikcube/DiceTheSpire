using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IOnInspectListener
{
    public Task OnInspectAsync(PlayerChoiceContext choiceContext, int cards, CardModel[] selectedCards, Player inspector);
}