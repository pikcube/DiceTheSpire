using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
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

        Parent.Flash();

        LocString locString = DiceySelection.ToBump;
        CardSelectorPrefs cardSelectorPrefs = new(locString, Power);
        IEnumerable<CardModel> result = await CardSelectCmd.FromHand(choiceContext, Parent.Owner,
            cardSelectorPrefs, null, this);

        foreach (CardModel card in result)
        {
            await card.BumpAsync(choiceContext);
        }
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        return AfterPlayerTurnStartLate(choiceContext, player);
    }
}