using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheWarrior.TheWarriorCode.Cards.Rare
{
    public class DiscoBall() : TheWarriorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {

        private int _testEnergyCostOverride = -1;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public int TestEnergyCostOverride
        {
            get => this._testEnergyCostOverride;
            set
            {
                TestMode.AssertOn();
                this.AssertMutable();
                this._testEnergyCostOverride = value;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Godot.Color("6ec46f"));
            await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.IntValue, Owner);
            foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.Where<CardModel>((Func<CardModel, bool>)(c => !c.EnergyCost.CostsX)))
            {
                if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
                {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(this.NextEnergyCost());
                    NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
                }
            }
        }

        private int NextEnergyCost()
        {
            return this.TestEnergyCostOverride >= 0 ? this.TestEnergyCostOverride : this.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}