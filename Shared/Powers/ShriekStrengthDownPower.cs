using BaseLib.Abstracts;
using DiceTheSpire.Warrior.Rare;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;

public class ShriekStrengthDownPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Shriek>();

    protected override bool IsPositive => false;
}