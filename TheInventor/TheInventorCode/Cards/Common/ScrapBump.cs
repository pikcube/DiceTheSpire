using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Common;

public class ScrapBump() : TheInventorCard(0, CardType.Skill, CardRarity.Common, TargetType.Self), IScrapCard
{ 
    public bool IsAlwaysOfferedAsScrap => true;

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => IsUpgraded ? [HoverTipFactory.Static(BetterStaticHoverTips.Bump)] : [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        LocString locString = IsUpgraded ? 
            DiceySelection.ToBump : 
            CardSelectorPrefs.UpgradeSelectionPrompt;
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