using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheInventor.TheInventorCode.Gadgets;

public class Accelerate() : GadgetModel(nameof(Accelerate))
{
    public override decimal PowerBase => 4;
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (Parent is null)
        {
            return;
        }

        if (Parent.Owner != player)
        {
            return;
        }

        await Parent.Owner.InspectAsync(choiceContext, Power);
    }
}