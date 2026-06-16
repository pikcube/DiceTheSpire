//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TheWarrior.TheWarriorCode.Cards.Common
//{
//    using BaseLib.Extensions;
//    using MegaCrit.Sts2.Core.Commands;
//    using MegaCrit.Sts2.Core.Entities.Cards;
//    using MegaCrit.Sts2.Core.Entities.Creatures;
//    using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//    using MegaCrit.Sts2.Core.Localization.DynamicVars;
//    using MegaCrit.Sts2.Core.Models;
//    using MegaCrit.Sts2.Core.Nodes.Cards;
//    using MegaCrit.Sts2.Core.ValueProps;

//    namespace TheWarrior.TheWarriorCode.Cards.Rare
//    {

//        public class OldIronShield() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
//        {

//            protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedBlock(0, Bonus)];

//            public override bool GainsBlock => true;

//            private static decimal Bonus(CardModel card, Creature? arg2)
//            {

//                if (card.Owner.PlayerCombatState is null)
//                {
//                    return 0;
//                }

//                int block = 3;

//                if (card.IsUpgraded)
//                {
//                    block += 3;
//                }

//                List<CardModel> handCards = [.. card.Owner.PlayerCombatState.Hand.Cards];
//                foreach (CardModel c in handCards)
//                {
//                    int xValue = c.EnergyCost.GetAmountToSpend();

//                    block += xValue;
//                }
//                return block;
//            }


//            protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//            {
//                await base.OnPlay(choiceContext, cardPlay);

//                await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(DynamicVars.CalculatedBlock.IntValue, BlockProps.card), cardPlay);
//            }

//            protected override void OnUpgrade()
//            {

//            }

//        }
//    }
//}

//  "THEWARRIOR-IRON_SHIELD.description": "Gain block equal to the energy cost\nof your hand {IfUpgraded:show:+ 6|+ 3}.\n(Currently {CalculatedBlock:diff()})",
//  "THEWARRIOR-IRON_SHIELD.title": "Iron Shield"