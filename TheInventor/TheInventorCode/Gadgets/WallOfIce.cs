using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class WallOfIce() : GadgetModel(nameof(WallOfIce))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override decimal PowerBase => 25;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (Parent is null || Parent.Owner.Creature != target)
        {
            return 1;
        }

        decimal val = 1;
        val *= (100M - Power) / 100M;

        return val;
    }
}