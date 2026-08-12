using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;


namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
public class IsAGoodOffensePower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if(result is null || dealer != Owner)
        {  
            return; 
        }
        await CreatureCmd.GainBlock(Owner, result.UnblockedDamage, BlockProps.nonCardUnpowered, null);
        await PowerCmd.Remove(this);
    }

    //public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    //{

    //    if (cardPlay.Card.Type != CardType.Attack)
    //    {
    //        return;
    //    }
    //    await PowerCmd.Remove(this);
    //}
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        await PowerCmd.Remove(this);
    }
}
