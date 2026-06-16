using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class DialUpSounds() : GadgetModel(nameof(DialUpSounds))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override bool IsAllowedAsTempGadget => false;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (Parent?.Owner.Creature is not null)
        {
            await BufferPower.ApplyAsync(new BlockingPlayerChoiceContext(), Parent.Owner.Creature, Power, Parent.Owner.Creature,
                null);
        }

        BreakMe();
    }

}