using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Thief.Rare;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class LeakyPipeStrengthDownPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<LeakyPipe>();

    protected override bool IsPositive => false;
}