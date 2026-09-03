using DiceTheSpire.DiceTheSpireCode.Thief.Uncommon;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class CactusNeedlePower : TemporaryThornsPower
{
    public override AbstractModel OriginModel => ModelDb.Card<CactusNeedle>();
}