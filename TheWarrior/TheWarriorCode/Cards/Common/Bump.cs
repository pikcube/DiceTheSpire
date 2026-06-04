using DiceTheSpireCore.DiceTheSpireCoreCode;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheWarrior.TheWarriorCode.Cards.Common;



 
public class Bump() : TheWarriorCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await base.OnPlay(choiceContext, cardPlay);
        LocString locString = new("card_selection", "TO_BUMP");
        CardSelectorPrefs cardSelectorPrefs = new(locString, this.DynamicVars.Cards.IntValue);
        IEnumerable<CardModel> result = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);

        foreach (CardModel card in result)
        {
            await card.BumpAsync();
        }
    }

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        this.DynamicVars.Cards.UpgradeValueBy(1);
    }

}