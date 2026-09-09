using BaseLib.Abstracts;
using DiceTheSpire.Warrior.Uncommon;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;

public class NightstickStrengthDownPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Nightstick>();

    protected override bool IsPositive => false;
}