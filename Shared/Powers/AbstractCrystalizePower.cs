using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public abstract class AbstractCrystalizePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public abstract bool ShouldUpgrade { get; }
    public sealed override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player) //AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner != player.Creature)
        {
            return;
        }

        IEnumerable<CardModel> crystals = ModelDb.AllCards.Where(c => c is ICrystalCard).TakeRandom(Amount, player.RunState.Rng.CombatCardGeneration);

        foreach (CardModel canonicalCard in crystals)
        {
            CardModel card = CombatState.CreateCard(canonicalCard, player);
            if (ShouldUpgrade)
            {
                CardCmd.Upgrade(card);
            }
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player));
        }
    }
}