using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Cards;


    [Pool(typeof(TokenCardPool))]
    public class RollAgain() : DiceTheSpireCoreCard(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {

        private int _testEnergyCostOverride = -1;
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];

        public int TestEnergyCostOverride
        {
            get => _testEnergyCostOverride;
            set
            {
                TestMode.AssertOn();
                AssertMutable();
                _testEnergyCostOverride = value;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            //NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Color("6ec46f"));

                CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_RANDOMIZE"), DynamicVars.Cards.IntValue, DynamicVars.Cards.IntValue);
                CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
                foreach (CardModel card in cards.Where(c => !c.EnergyCost.CostsX))
                {
                    if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
                    {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(NextEnergyCost());
                    await Cmd.Wait(0.5f);
                    NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
                    }
                }

        }
        

        private int NextEnergyCost()
        {
            return TestEnergyCostOverride >= 0 ? TestEnergyCostOverride : Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Ethereal);
            AddKeyword(CardKeyword.Retain);
        }
    
    }

