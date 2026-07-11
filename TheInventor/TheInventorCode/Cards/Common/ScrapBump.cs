using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Keywords;

namespace TheInventor.TheInventorCode.Cards.Common;

//Scrap Bump (Common Skill) [0] Upgrade a (2) card(s). Scrap.
//Auto Bump (Gadget): At the start of each turn, upgrade a random card.

//Weaker version of Bump (Warrior or Thief), which will allow you to also pick upgraded cards to create unupgraded copies.
public class ScrapBump() : TheInventorCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ScrapKeyword.Scrap];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => IsUpgraded ? [HoverTipFactory.Static(BetterStaticHoverTips.Bump)] : [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        LocString locString = IsUpgraded ? 
            new LocString("card_selection", "TO_BUMP") : 
            new LocString("gameplay_ui", "CHOOSE_CARD_UPGRADE_HEADER");
        CardSelectorPrefs cardSelectorPrefs = new(locString, 1);
        IEnumerable<CardModel> result = await CardSelectCmd.FromHand(choiceContext, Owner,
            cardSelectorPrefs,
            model => IsUpgraded || model.IsUpgradable, this);

        foreach (CardModel card in result)
        {
            await card.BumpAsync(choiceContext);
        }
    }

    public override string GetScrapId => nameof(AutoBump);
}