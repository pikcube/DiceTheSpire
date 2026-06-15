using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Gadgets;

public class AutoBump() : GadgetModel(nameof(AutoBump))
{
    public override decimal PowerBase => 1;
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player)
        {
            return;
        }

        LocString locString = new("card_selection", "TO_BUMP");
        CardSelectorPrefs cardSelectorPrefs = new(locString, Power);
        IEnumerable<CardModel> result = await CardSelectCmd.FromHand(choiceContext, Parent.Owner,
            cardSelectorPrefs, null, this);

        foreach (CardModel card in result)
        {
            await card.BumpAsync();
        }
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStartLate(choiceContext, player);
    }
}