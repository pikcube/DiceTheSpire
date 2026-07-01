using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheThief.TheThiefCode.Cards.Common;

namespace TheThief.TheThiefCode.Powers;

public class BenchPressPower : TemporaryStrengthPower, ICustomModel
{
    public override AbstractModel OriginModel => ModelDb.Card<BenchPress>();
}