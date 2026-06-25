using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Powers;

public class BackfirePower : TheInventorPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;

    private Stack<decimal> Amounts { get; set; } = [];

    private bool IsStupid { get; set; } //A hack to avoid overflowing the stack. Set this to true to prevent this power from modifying damage that results from decreasing your max hp.

    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner || Owner.HasPower<BufferPower>() || IsStupid)
        {
            return amount;
        }

        Amounts.Push(amount);

        return 0;
    }

    public override async Task AfterModifyingHpLostAfterOsty()
    {
        if (Amounts.TryPop(out decimal amount))
        {
            HookPlayerChoiceContext context = new(this, LocalContext.NetId ?? 0, CombatState, GameActionType.Combat);
            IsStupid = true;
            await CreatureCmd.LoseMaxHp(context, Owner, amount, false);
            IsStupid = false;
        }
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        Amounts = [];
        return Task.CompletedTask;
    }
}