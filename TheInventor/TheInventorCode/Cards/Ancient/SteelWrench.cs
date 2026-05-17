using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Ancient;

public class SteelWrench() : TheInventorCard(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await base.OnPlay(choiceContext, cardPlay);
        CardSelectorPrefs discardPrefs = new(new LocString("cards", "THEINVENTOR-STEEL_WRENCH.prompt"), 2);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, discardPrefs, null, this);
        await CardCmd.DiscardAndDraw(choiceContext, cards, DynamicVars.Cards.IntValue);
    }

    public override string OnScrap()
    {
        return nameof(DefaultGadget);
    }
}