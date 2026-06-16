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
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class PuppyPaws() : TheInventorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(AutoBump);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(10, BlockProps.card)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
        [HoverTipFactory.Static(BetterStaticHoverTips.Bump)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        if (IsUpgraded)
        {
            foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.ToArray())
            {
                await card.BumpAsync();
            }
        }
        else
        {
            LocString locString = new("card_selection", "TO_BUMP");
            CardSelectorPrefs cardSelectorPrefs = new(locString, 1);
            IEnumerable<CardModel> result = await CardSelectCmd.FromHand(choiceContext, Owner,
                cardSelectorPrefs, null, this);

            foreach (CardModel card in result)
            {
                await card.BumpAsync();
            }
        }
    }

    protected override void OnUpgrade()
    {
        
    }
}