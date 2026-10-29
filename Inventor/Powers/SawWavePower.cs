using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Powers;

[UsedImplicitly]
public class SawWavePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.None;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (target == Owner && Owner.Powers.Any(p => DiceyHelperFunctions.IsDebuffAtAmount(p, p.Amount)))
        {
            return 0.75m;
        }

        return 1;
    }
}