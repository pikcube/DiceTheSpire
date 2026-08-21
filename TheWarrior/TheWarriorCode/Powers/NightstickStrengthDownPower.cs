using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWarrior.TheWarriorCode.Cards.Uncommon;

namespace TheWarrior.TheWarriorCode.Powers;

public class NightstickStrengthDownPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Nightstick>();

    protected override bool IsPositive => false;
}