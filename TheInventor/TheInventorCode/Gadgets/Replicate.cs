using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Gadgets;

public class Replicate() : GadgetModel(nameof(Replicate))
{
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

        CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_DUPE"), 1, 1);
        CardModel? result = (await CardSelectCmd.FromHand(choiceContext, player, prefs, null, this)).SingleOrDefault();
        if (result is null)
        {
            return;
        }

        CardModel copy = result.CreateClone();
        await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, player);
    }
}