using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SmartFormat.Core.Extensions;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;


public class PermafrostRetentionPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
    {
        if(retainedCards is null)
        {
            return;
        }

        //CardModel[] cards = (CardModel[])retainedCards;
        foreach (CardModel card in retainedCards)
        {
            await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
        } 
    }
}