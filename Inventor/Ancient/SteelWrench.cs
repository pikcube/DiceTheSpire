using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Ancient;

[UsedImplicitly]
public class SteelWrench() : TheInventorCard(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int handSize = PileType.Hand.GetPile(Owner).Cards.Count;

        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToShock, 0, handSize);
        CardModel[] cards = [..await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        foreach (CardModel card in cards)
        {
            await card.ShockAsync(choiceContext);
        }

        if (cards.Length == 0)
        {
            return;
        }

        CardSelectorPrefs prefs = new(DiceySelection.ToPull, cards.Length);
        IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, prefs);

        foreach (CardModel result in results)
        {
            await CardPileCmd.Add(result, PileType.Hand);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public override string GetScrapId => nameof(BattleWrench);
}