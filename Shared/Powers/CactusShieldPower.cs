using DiceTheSpire.Thief.Common;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class CactusShieldPower: TemporaryThornsPower
{
    public override AbstractModel OriginModel => ModelDb.Card<CactusShield>();
}