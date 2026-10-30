using BaseLib.Abstracts;
using DiceTheSpire.Thief.Uncommon;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;

public class DeterminationDexterityPower : TemporaryDexterityPower, ICustomModel
{
    public override AbstractModel OriginModel => ModelDb.Card<Determination>();
}