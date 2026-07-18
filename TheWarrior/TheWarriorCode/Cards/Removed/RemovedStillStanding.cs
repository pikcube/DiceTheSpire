//using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
//using MegaCrit.Sts2.Core.Combat;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.Entities.Creatures;
//using MegaCrit.Sts2.Core.Entities.Players;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.Models;
//using MegaCrit.Sts2.Core.Models.Powers;
//using MegaCrit.Sts2.Core.Nodes.Cards;
//using MegaCrit.Sts2.Core.Nodes.Combat;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Godot.HttpRequest;

//"THEWARRIOR-STILL_STANDING.description": "WIP (become unkillable lol)",
//  "THEWARRIOR-STILL_STANDING.title": "Still Standing"

//namespace TheWarrior.TheWarriorCode.Cards.Rare
//{
//    public class StillStanding() : TheWarriorCard(0, CardType.Curse, CardRarity.Rare, TargetType.Self)
//    {
//        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
//        public override bool ShouldDie(Creature creature) => creature != Owner.Creature;

//        public int isStillStanding = 1;
//        public override async Task AfterPreventingDeath(Creature creature)
//        {
//            if (creature != Owner.Creature || Owner.PlayerCombatState is null)
//            {
//                return;
//            }
//            StillStanding stillStanding = this;
//            //List<CardModel> exhaustPile = [.. Owner.PlayerCombatState.ExhaustPile.Cards];


//            await CreatureCmd.Heal(creature, 20M, false);
//            isStillStanding = isStillStanding--;
//            if (isStillStanding < 0)
//            {
//                await CreatureCmd.Kill(Owner.Creature);
//            }
//            return;
//        }
//    }
//}
