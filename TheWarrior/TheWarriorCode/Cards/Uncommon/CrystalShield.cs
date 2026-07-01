using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;


namespace TheWarrior.TheWarriorCode.Cards.Uncommon
{




    public class CrystalShield() : TheWarriorCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {

        protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedBlock(0, Bonus)];

        public override bool GainsBlock => true;

        private static decimal Bonus(CardModel card, Creature? arg2)
        {

            if (card.Owner.PlayerCombatState is null)
            {
                return 0;
            }

            int block = 0;

            List<CardModel> handCards = [.. card.Owner.PlayerCombatState.Hand.Cards];
            foreach (CardModel c in handCards)
            {
                int xValue = c.EnergyCost.GetAmountToSpend();

                block += xValue;
            }
            return block;
        }


        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            CrystalShield crystalShield = this;
            if (DynamicVars.CalculatedBlock is null)
            {
                return;
            }
            await base.OnPlay(choiceContext, cardPlay);
            await CreatureCmd.GainBlock(crystalShield.Owner.Creature, crystalShield.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), crystalShield.DynamicVars.CalculatedBlock.Props, cardPlay);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }

    }

}

