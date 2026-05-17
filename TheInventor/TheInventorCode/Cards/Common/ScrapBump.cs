using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await base.OnPlay(choiceContext, cardPlay);
        IEnumerable<CardModel> result = await CardSelectCmd.FromHand(choiceContext, Owner,
            new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_UPGRADE_HEADER"), DynamicVars.Cards.IntValue),
            model => model.IsUpgradable, this);

        foreach (CardModel card in result)
        {
            CardCmd.Upgrade(card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public override string OnScrap()
    {
        return nameof(AutoBump);
    }
}