//using DiceTheSpireCore.DiceTheSpireCoreCode;
//using Godot;
//using MegaCrit.Sts2.Core.CardSelection;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.HoverTips;
//using MegaCrit.Sts2.Core.Localization;
//using MegaCrit.Sts2.Core.Localization.DynamicVars;
//using MegaCrit.Sts2.Core.Models;
//using MegaCrit.Sts2.Core.Models.Cards;
//using MegaCrit.Sts2.Core.Nodes.Cards;
//using MegaCrit.Sts2.Core.Nodes.Rooms;
//using MegaCrit.Sts2.Core.TestSupport;

//namespace TheWarrior.TheWarriorCode.Cards.Uncommon
//{

//public class Candle() : TheWarriorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
//    {

//        private int _testEnergyCostOverride = -1;

//        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new RepeatVar(3)];
//        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.RerollAsync)];

//        public int TestEnergyCostOverride
//        {
//            get => _testEnergyCostOverride;
//            set
//            {
//                TestMode.AssertOn();
//                AssertMutable();
//                _testEnergyCostOverride = value;
//            }
//        }

//        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//        {
//            NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Color("6ec46f"));

//            for (int n = 0; n < DynamicVars.Repeat.IntValue; ++n)
//            {
//                CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_RANDOMIZE"), 0, DynamicVars.Cards.IntValue);
//                CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
//                foreach (CardModel card in cards.Where(c => !c.EnergyCost.CostsX))
//                {
//                    if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
//                    {
//                        card.EnergyCost.SetThisTurnOrUntilPlayed(NextEnergyCost());
//                        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
//                    }
//                }
//                await Cmd.Wait(0.5f);
 
//            }
//            if(CombatState is null)
//            {
//                return;
//            }

//            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Burn>(Owner), PileType.Draw, Owner));
//            await Cmd.Wait(0.5f);
//        }

//        private int NextEnergyCost()
//        {
//            return TestEnergyCostOverride >= 0 ? TestEnergyCostOverride : Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
//        }

//        protected override void OnUpgrade()
//        {
//            DynamicVars.Repeat.UpgradeValueBy(1);
//        }
//    }
//}

