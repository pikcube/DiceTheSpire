using DiceTheSpire.Thief.Common;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;

public class ParryingDaggerPower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<ParryingDagger>();
}