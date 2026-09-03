using BaseLib.Abstracts;
using DiceTheSpire.Thief.Common;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Common.Powers;

public class BellPeppersStrengthPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<BellPeppers>();
}