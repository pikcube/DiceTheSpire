using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Warrior.Uncommon;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class NightstickStrengthDownPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Nightstick>();

    protected override bool IsPositive => false;
}