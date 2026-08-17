using BaseLib.Utils;
using DiceTheSpireCore.DiceTheSpireCoreCode.Commands;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Cards;


[Pool(typeof(TokenCardPool))]
public class RollAgain() : DiceTheSpireCoreCard(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToRandomize, DynamicVars.Cards.IntValue, DynamicVars.Cards.IntValue);
        CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        foreach (CardModel card in cards.Where(c => !c.EnergyCost.CostsX && c.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0))
        {
            await RerollCmd.RerollAsync(card, RerollDuration.UntilEndOfTurnOrPlayed);
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
        AddKeyword(CardKeyword.Retain);
    }

}