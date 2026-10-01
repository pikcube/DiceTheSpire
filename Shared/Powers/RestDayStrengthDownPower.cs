using BaseLib.Abstracts;
using DiceTheSpire.Warrior.Uncommon;
using DiceTheSpire.Warrior.WorkoutRewards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;

public class RestDayStrengthDownPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<RestDay>();

    protected override bool IsPositive => false;
}