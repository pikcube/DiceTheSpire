using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Common.Powers;


public class PlasmaCannonPower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<GrindstonePower>()];
    public int UsesThisTurn { get; set; } = 0;

    public override async Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier,
        CardModel? cardSource)
    {
        if (applier != Owner || Owner.Player is null || power.GetTypeForAmount(amount) != PowerType.Debuff)
        {
            return;
        }



        HookPlayerChoiceContext choiceContext = new(Owner.Player, LocalContext.NetId ?? 0, GameActionType.Combat);

        await GrindstonePower.ApplyAsync(choiceContext, Owner, Amount, Owner, cardSource);
    }
}