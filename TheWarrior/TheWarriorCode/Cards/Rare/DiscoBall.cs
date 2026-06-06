using DiceTheSpireCore.DiceTheSpireCoreCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;


namespace TheWarrior.TheWarriorCode.Cards.Rare
{

public class DiscoBall() : TheWarriorCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {

        private int _testEnergyCostOverride = -1;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
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
            NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Godot.Color("6ec46f"));
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
            foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.Where(c => !c.EnergyCost.CostsX))
            {
                if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
                {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(NextEnergyCost());
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
            //RemoveKeyword(CardKeyword.Exhaust);
            DynamicVars.Cards.UpgradeValueBy(2);
        }
    }
}