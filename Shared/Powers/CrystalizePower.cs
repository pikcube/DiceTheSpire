using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiceTheSpire.Shared.Powers;

public class CrystalizePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player) //AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (Owner != player.Creature)
            {
                return;
            }

            IEnumerable<CardModel> crystals = ModelDb.AllCards.Where(c => c is ICrystalCard).TakeRandom(Amount, player.RunState.Rng.CombatCardGeneration);

            foreach(CardModel card in crystals)
            {
                CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card,PileType.Hand, player));
            }
        }
 }

