using DiceTheSpire.Thief.Common;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class ParryingDaggerPower : TemporaryReducePower
{
    public override AbstractModel OriginModel => ModelDb.Card<ParryingDagger>();
}