using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Gadgets;

[UsedImplicitly]
public class WallOfIce() : GadgetModel(nameof(WallOfIce))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override decimal PowerBase => 25;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (Parent is null || Parent.Owner.Creature != target)
        {
            return 1;
        }

        decimal val = 1;
        val *= (100M - Power) / 100M;

        if (val < 0)
        {
            val = 0;
        }

        return val;
    }

    public override Task AfterModifyingDamageAmount(CardModel? cardSource)
    {
        Parent?.Flash();
        return Task.CompletedTask;
    }
}