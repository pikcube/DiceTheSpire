using DiceTheSpire.DiceTheSpireCode.Thief.Common;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class CloakPower: TemporaryReducePower
{
    public override AbstractModel OriginModel => ModelDb.Card<Cloak>();
}