using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

public class Efficiency() : GadgetModel(nameof(Efficiency))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public int Count { get; set; }

    public override void OnFirstCharge()
    {
        Parent?.SetValue(Power);
    }


    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Count = 0;
        return Task.CompletedTask;
    }

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (Parent?.Owner != card.Owner || Count >= Power || isAutoPlay || card.Type == CardType.Power)
        {
            return cardLocation;
        }

        ++Count;

        Parent.Flash();

        Parent.SetValue(Count >= Power ? 0 : Power - Count);
        return new CardLocation(card.Owner, PileType.Hand, CardPilePosition.Top);
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        Count = 0;
        return Task.CompletedTask;
    }
}