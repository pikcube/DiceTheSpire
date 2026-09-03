using DiceTheSpire.DiceTheSpireCode.Thief.Common;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class ParryingDaggerPower : TemporaryReducePower
{
    public override AbstractModel OriginModel => ModelDb.Card<ParryingDagger>();
}