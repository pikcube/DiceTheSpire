using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Gadgets;

public class Hook() : GadgetModel(nameof(Hook))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent is null)
        {
            return;
        }

        if (Parent?.Owner != player)
        {
            return;
        }

        CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_PULL"), Power);
        CardModel? card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Parent.Owner), Parent.Owner, prefs)).FirstOrDefault();
        if (card == null)
        {
            return;
        }
        await CardPileCmd.Add(card, PileType.Hand);
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player) =>
        AfterPlayerTurnStartLate(choiceContext, player);
}