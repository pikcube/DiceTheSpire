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

namespace TheWarrior.TheWarriorCode.Cards.Common
{

public class Nudge() : TheWarriorCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new BlockVar(8, BlockProps.card)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await base.OnPlay(choiceContext, cardPlay);
            LocString locString = new("card_selection", "TO_NUDGE");
            CardSelectorPrefs cardSelectorPrefs = new(locString, DynamicVars.Cards.IntValue);
            IEnumerable<CardModel> result = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);

            foreach (CardModel card in result)
            {
                if(card.CurrentUpgradeLevel > 0)
                {
                    await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
                }
                    await card.NudgeAsync(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            base.OnUpgrade();
            DynamicVars.Cards.UpgradeValueBy(1);
        }

    }
}
