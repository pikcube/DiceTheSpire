using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpire.DiceTheSpireCode.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Rare;


public class Befuddle() : TheInventorCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Replicate);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToBlink, 1, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)).SingleOrDefault();
        if (card is null)
        {
            return;
        }

        await card.BlinkAsync(choiceContext);

        BefuddlePower? power = await BefuddlePower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Cards.IntValue, Owner.Creature, this);

        power?.SetCards(card);
    }
}