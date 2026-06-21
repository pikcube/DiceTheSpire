using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Models;
using TheThief.TheThiefCode.Cards.Common;

namespace TheThief.TheThiefCode.Powers;

public class ParryingDaggerPower : TemporaryReducePower
{
    public override AbstractModel OriginModel => ModelDb.Card<ParryingDagger>();
}