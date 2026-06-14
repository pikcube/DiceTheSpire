using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheThief.TheThiefCode.Cards.Rare;

namespace TheThief.TheThiefCode.Powers;

public class LeakyPipeStrengthDownPower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<LeakyPipe>();

    protected override bool IsPositive => false;
}