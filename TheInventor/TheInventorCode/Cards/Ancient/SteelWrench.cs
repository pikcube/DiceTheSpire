using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Ancient;

public class SteelWrench() : TheInventorCard(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await base.OnPlay(choiceContext, cardPlay);
        CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_BLINK"), 2, 2);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);
        foreach (CardModel card in cards)
        {
            await card.BlinkAsync(choiceContext);
        }
        await CardCmd.DiscardAndDraw(choiceContext, [], DynamicVars.Cards.IntValue);

        
    }

    public override string OnScrap()
    {
        return nameof(DefaultGadget);
    }
}