using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;


public class PermafrostRetentionPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    //public async Task AfterCardRetained(PlayerChoiceContext choiceContext, CardModel card)
    //{
    //    if (card.Owner.Creature != Owner)
    //    {
    //        return;
    //    }
    //    await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
    //}

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side || Owner.Player is null)
        {
        }

        //foreach (CardModel _ in PileType.Hand.GetPile(Owner.Player).Cards.Equals(CardKeyword.Retain))
        //{
        //    await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
        //}

    }
}