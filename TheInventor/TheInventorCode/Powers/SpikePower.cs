using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Powers;

public class SpikePower : TheInventorPower, IAfterCardShockedListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner.Creature != Owner || card.Pile?.Type != PileType.Hand || oldPileType == PileType.Hand || !card.Keywords.Contains(CardKeyword.Unplayable))
        {
            return;
        }

        HookPlayerChoiceContext hookPlayerChoiceContext = new(this, card.Owner.NetId, CombatState, GameActionType.Combat);
        await CreatureCmd.Damage(hookPlayerChoiceContext, CombatState.Enemies, Amount, DamageProps.nonCardUnpowered, Owner, null, null);
    }

    public async Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner.Creature == Owner && card.Pile?.Type == PileType.Hand)
        {
            await CreatureCmd.Damage(choiceContext, CombatState.Enemies, Amount, DamageProps.nonCardUnpowered, Owner, null, null);
        }
    }
}