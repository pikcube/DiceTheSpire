using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Ancient;

[UsedImplicitly]
public class SteelWrench() : TheInventorCard(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int handSize = PileType.Hand.GetPile(Owner).Cards.Count;

        CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_BLINK"), 0, handSize);
        CardModel[] cards = [..await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        foreach (CardModel card in cards)
        {
            await card.BlinkAsync(choiceContext);
        }

        if (cards.Length == 0)
        {
            return;
        }

        CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_PULL"), IsUpgraded ? cards.Length + 1 : cards.Length);
        IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, prefs);

        foreach (CardModel result in results)
        {
            await CardPileCmd.Add(result, PileType.Hand);
        }
    }

    public override string GetScrapId => nameof(BattleWrench);
}