using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Singletons;

public class DiscardSingletonModel() : CustomSingletonModel(HookType.Combat)
{
    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card is IOnDiscardListener listener)
        {
            await listener.OnDiscardAsync(choiceContext);
        }
    }
}

public interface IOnDiscardListener
{
    public Task OnDiscardAsync(PlayerChoiceContext choiceContext);
}