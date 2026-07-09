using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheThief.TheThiefCode.Cards.Uncommon;

namespace TheThief.TheThiefCode.Powers;

public class OverwhelmStrengthPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Overwhelm>();
}