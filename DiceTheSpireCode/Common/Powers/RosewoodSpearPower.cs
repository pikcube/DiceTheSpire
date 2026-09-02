using DiceTheSpire.DiceTheSpireCode.Thief.Common;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class RosewoodSpearPower : TemporaryThornsPower
{
    public override AbstractModel OriginModel => ModelDb.Card<RosewoodSpear>();
}
