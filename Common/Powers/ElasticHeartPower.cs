using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Common.Powers;


public class ElasticHeartPower : DiceTheSpireCorePower
{
    private const UnplayableReason ValidReasons = UnplayableReason.HasUnplayableKeyword | UnplayableReason.BlockedByHook;
    private static bool IsCardUnplayable(CardModel card)
    {
        card.CanPlay(out UnplayableReason reason, out _);
        return (ValidReasons & reason) > 0;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side || Owner.Player is null)
        {
            return;
        }

        foreach (CardModel _ in PileType.Hand.GetPile(Owner.Player).Cards.Where(IsCardUnplayable))
        {
            await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
        }
    }
}