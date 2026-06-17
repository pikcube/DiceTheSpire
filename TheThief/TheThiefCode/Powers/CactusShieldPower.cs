using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheThief.TheThiefCode.Cards.Common;

namespace TheThief.TheThiefCode.Powers;

public class CactusShieldPower: TemporaryThornsPower
{
    public override AbstractModel OriginModel => ModelDb.Card<CactusShield>();
}