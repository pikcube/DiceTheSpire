using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards.Common;

namespace TheInventor.TheInventorCode.Powers;


public class FreezePower : TheInventorPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        await PowerCmd.Apply<DexterityPower>(new BlockingPlayerChoiceContext(), target, -amount, applier, cardSource);
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        await PowerCmd.Apply<DexterityPower>(new BlockingPlayerChoiceContext(), Owner, Amount, Applier, null);
        await PowerCmd.Remove(this);
    }
}