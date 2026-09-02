using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Thief.Uncommon;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class OverwhelmStrengthPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Overwhelm>();
}