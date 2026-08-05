using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheThief.TheThiefCode.Cards.Common;

namespace TheThief.TheThiefCode.Powers;

public class BellPeppersStrengthPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<BellPeppers>();
}