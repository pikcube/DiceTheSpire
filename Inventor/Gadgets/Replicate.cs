using BaseLib.Abstracts;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Gadgets;

public class Replicate() : GadgetModel(nameof(Replicate))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStart(choiceContext, player);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player)
        {
            return;
        }

        Parent.Flash();
        CardSelectorPrefs prefs = new(DiceySelection.ToDupe, Power, Power);

        foreach (CardModel result in await CardSelectCmd.FromHand(choiceContext, player, prefs, null, this))
        {
            CardModel copy = result.CreateClone();
            await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, player);
        }
    }
}