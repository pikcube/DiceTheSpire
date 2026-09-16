using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class HauntPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Player player = cardPlay.Player;
        if (player.Creature != Owner)
        {
            return;
        }

        CardPile hand = PileType.Hand.GetPile(player);

        CardModel[] cardsToShock = [.. hand.Cards.TakeRandom(Amount, player.RunState.Rng.CombatCardSelection)];
        await ShockModel.ShockAndReplaceAsync(choiceContext, player, cardsToShock, cardsToShock.Length);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
        {
            return;
        }
        await PowerCmd.Remove(this);
    }
}